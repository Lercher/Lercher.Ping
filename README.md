# Lercher.Ping
a simple ping state logger

Logs something like this to the file `ping.log`
````
2017-11-23T19:16:16 192.168.1.34 *START*
2017-11-23T19:16:49 192.168.1.34 OK
2017-11-23T19:17:14 192.168.1.34 *LAST*
2017-11-23T19:17:19 192.168.1.34 *START*
2017-11-23T19:17:19 192.168.1.34 OK
2017-11-23T19:17:29 192.168.1.34 TimedOut
2017-11-23T19:17:31 192.168.1.34 *LAST*
2017-11-23T19:21:48 192.168.1.34 *START*
2017-11-23T19:21:49 192.168.1.34 TimedOut
2017-11-23T19:22:00 192.168.1.34 *LAST*
````