// Based upon: http://netfilter.org/projects/libnetfilter_log/doxygen/nfulnl__test_8c_source.html

#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <netinet/in.h>
#include <libnetfilter_log/libnetfilter_log.h>
#include <errno.h>
#include <inttypes.h>

struct nflog_handle *h;
struct nflog_g_handle *qh;
struct nflog_g_handle *qh100;

void TerminateSafely()
{
    printf("unbinding from group 100\n");
    nflog_unbind_group(qh100);
    printf("unbinding from group 0\n");
    nflog_unbind_group(qh);
    printf("closing handle\n");
    nflog_close(h);
    exit(0);
}

static int cb(struct nflog_g_handle *gh, struct nfgenmsg *nfmsg,
              struct nflog_data *nfa, void *data)
{
    // char buffer [8192];
    // nflog_snprintf_xml(buffer, 8192, nfa, NFLOG_XML_PAYLOAD);
    // printf("%s\n", buffer);
    // fflush(stdout);

    char* hex;
    int payloadLength = nflog_get_payload(nfa, &hex);

    if(payloadLength < 24) { return 0; }

    printf("%hhu.%hhu.%hhu.%hhu|", hex[12], hex[13], hex[14], hex[15]);
    printf("%hhu.%hhu.%hhu.%hhu|", hex[16], hex[17], hex[18], hex[19]);

    char bSrcPort[2];
    bSrcPort[0] = hex[21];
    bSrcPort[1] = hex[20];
    
    char bDstPort[2];
    bDstPort[0] = hex[23];
    bDstPort[1] = hex[22];


    //printf("%hhu | %hhu | %hhu | %hhu | %hhu | %hhu \n", hex[20], hex[21], hex[22], hex[23], hex[24], hex[25]);

    uint16_t sourcePort = 0;
    uint16_t destinationPort = 0;

    char* ptr = &sourcePort;
    *(ptr + 0) = hex[21];
    *(ptr + 1) = hex[20];

    ptr = &destinationPort;
    *(ptr + 0) = hex[23];
    *(ptr + 1) = hex[22];

    char *prefix = nflog_get_prefix(nfa);


    printf("%" PRIu16 "|%" PRIu16 "|%s\n", sourcePort, destinationPort, prefix);
    fflush(stdout);
}

void diedie(int sig)
{
    printf("Told to terminate \n");
    TerminateSafely();
}

int main(int argc, char **argv)
{
    struct sigaction action;
    action.sa_handler = diedie;
    sigemptyset(&action.sa_mask);
    action.sa_flags = 0;
    
    if(sigaction(SIGINT, &action, NULL))
    {
        perror("Could not register sigaction SIGINT");
    }
    
    if(sigaction(SIGTERM, &action, NULL))
    {
        perror("Could not register sigaction SIGTERM");
    }

    int rv, fd;
    char buf[8192];
    //char* buf = malloc(10485760);

    

    h = nflog_open();
    if (!h) {
        fprintf(stderr, "error during nflog_open()\n");
        exit(1);
    }

    //printf("unbinding existing nf_log handler for AF_INET (if any)\n");
    if (nflog_unbind_pf(h, AF_INET) < 0) {
        perror("error nflog_unbind_pf()\n");
        //fprintf(stderr, "error nflog_unbind_pf()\n");
        exit(1);
    }

    //printf("binding nfnetlink_log to AF_INET\n");
    if (nflog_bind_pf(h, AF_INET) < 0) {
        fprintf(stderr, "error during nflog_bind_pf()\n");
        exit(1);
    }
    //printf("binding this socket to group 0\n");
    qh = nflog_bind_group(h, 0);
    if (!qh) {
        fprintf(stderr, "no handle for grup 0\n");
        exit(1);
    }

    //printf("binding this socket to group 1\n");
    qh100 = nflog_bind_group(h, 1);
    if (!qh100) {
        fprintf(stderr, "no handle for group 1\n");
        exit(1);
    }

    //printf("setting copy_packet mode\n");
    if (nflog_set_mode(qh, NFULNL_COPY_PACKET, 0xffff) < 0) {
        fprintf(stderr, "can't set packet copy mode\n");
        exit(1);
    }

    fd = nflog_fd(h);

    //printf("registering callback for group 0\n");
    nflog_callback_register(qh, &cb, NULL);

    //printf("going into main loop\n");
    fflush(stdout);
    while ((rv = recv(fd, buf, sizeof(buf), 0)) && rv >= 0) {
    //while ((rv = recv(fd, buf, 10485760, 0)) && rv >= 0) {
        struct nlmsghdr *nlh;
        //printf("pkt received (len=%u)\n", rv);

        /* handle messages in just-received packet */
        nflog_handle_packet(h, buf, rv);
        
    }

    printf("Finished Loop. RV: %i\n", rv);
    printf("Err no returned %i\n", errno);

    if(rv == -1)
    {
        perror("RV ERROR: ");
    }

    TerminateSafely();
}