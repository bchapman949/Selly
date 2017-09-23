#!/bin/sh

gcc -c main.c
libtool --mode=link gcc -L/usr/local/lib  main.c -o main -lnetfilter_log
