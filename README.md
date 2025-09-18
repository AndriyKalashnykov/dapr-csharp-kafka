# CSharp Dapr Sample with Kafka

This example shows how to run Dapr in CSharp with Kafka and Minimal APIs.

### Run Kafka

First, make sure you have Docker installed so you can run Kafka locally.

```bash
make runk
```

### Run the subscriber

```bash
make runs
```

### Run the publisher

```bash
make runp
```

### Observe the event

In your subscriber app you should see:

```bash
== APP == Id=8beb4163-1548-43ea-bea3-7726bc584131, Data=2025-09-17T12:28:31.2696216Z
```

links
* [Dapr Console](http://localhost:8080/overview)
* [Zipkin Console](http://127.0.0.1:9411/zipkin/)


### References

* [Kafka pubsub connection refused](https://github.com/dapr/components-contrib/issues/3113)
* [Dapr .NET Workflow examples](https://github.com/olitomlinson/dapr-workflow-testing)
* [Dapr Globo Tickets .NET Example](https://github.com/XpiritBV/azure-container-apps-workshop/tree/main/src/globo-tickets-dapr)
* [DAPR .NET Examples + Docker Compose](https://github.com/Sen-Gupta/nebulagraph/tree/main/src/examples)