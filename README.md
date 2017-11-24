# Lercher.Ping
A simple ping state logger. Its main feature is a space efficient log
file, suitable for long term monitoring.

Logs something like this to the file `ping.log`
````
2017-11-23T19:40:10 172.29.248.109 *START*
2017-11-23T19:40:11 172.29.248.109 OK within 100ms
2017-11-23T22:10:19 172.29.248.109 OK within 300ms
2017-11-23T22:10:29 172.29.248.109 OK within 100ms
2017-11-23T22:22:45 172.29.248.109 TimedOut
2017-11-23T22:24:45 172.29.248.109 OK within 100ms
2017-11-23T23:35:32 172.29.248.109 OK within 300ms
2017-11-23T23:35:43 172.29.248.109 OK within 100ms
2017-11-24T01:06:07 172.29.248.109 OK within 200ms
2017-11-24T01:06:28 172.29.248.109 OK within 100ms
...
2017-11-24T13:35:56 172.29.248.109 OK within 100ms
2017-11-24T13:45:39 172.29.248.109 OK within 200ms
2017-11-24T13:45:50 172.29.248.109 OK within 100ms
2017-11-24T13:51:52 172.29.248.109 *LAST*
````
