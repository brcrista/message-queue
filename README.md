# Message Queue

A durable message queue implemented using C# and SQLite.

## Building

```
dotnet build
```

## Usage

First, you must run the publisher. This will create the database and begin sending messages:

```shell
./MessageQueue.Publisher/bin/Debug/net6.0/MessageQueue.Publisher.exe
```

Then, running the subscriber will consume messages:

```shell
./MessageQueue.Subscriber/bin/Debug/net6.0/MessageQueue.Subscriber.exe
```

Once messages have been consumed, you can purge them with

```shell
./MessageQueue.Cleanup/bin/Debug/net6.0/MessageQueue.Cleanup.exe
```