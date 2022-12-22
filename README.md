# jh-code-challenge
An app to stream from Twitter API sample Tweet stream. 

The purpose of the app is to demonstrate technique and organization in how I write and organize code . Its function is:

1. Utilize the sample stream endpoint provided in the Twitter API.
2. Track Statistics on the recieved tweets. 
3. Provide a means of reporting the gathered statitics to the user

The provided metrics are:
* Total Number of Tweets*
* Top 10 Hashtags recieved*
* Total distinct Hashtags
* Top 10 Tweet Languages
* Total Distinct Tweet Languages

## Running the Program
Before running the program replace the Authorization bearer token in the appsettings.json file:

```json
"HttpClientConfig": {
  "AuthToken": "{{AUTH_TOKEN}}",
  "BaseUrl": "https://api.twitter.com/2/"
}
```

When debugging logs will be available in the debug output. 

## Design Considerations

### Description
The application will be a dotnet console applicaiton, that will run the stream component and the processing component in parallel.  The console will provide an on demand reporting  mechanism as well as an option to periodically refresh automatically.

There will be three main components to the application: A Twitter stream service, a Tweet processing service, and User interface to provide the reporting features.

[Component Diagram](https://lucid.app/lucidchart/de91193b-9a64-4f41-880e-f92d720cc386/edit?viewport_loc=-95%2C-31%2C1997%2C919%2C0_0&invitationId=inv_b96b4710-0691-496f-80e5-16a467195f8a)

#### Twitter Stream Service 
This service will: 
* Connect to the Twitter API through a client component
* Listen for a tweet recived event raised by the client
* Recieve the Tweet in the event args 
* Send each tweet to a Tweet Queue 

#### Tweet Processing Service
* Listen for an available tweet in the queue. (Possibly an event raised by the Queue client?)
* Dequeue the tweet object
* Send Tweet to the metrics processor
* send metrics to the metrics store.

#### User Interface
The reporting component begins writing tot he console after 10 seconds, and thereafter refreshes every ten seconds. 

## Additional Features 
1. Retry on connection failure
2. Add configuration for hard coded values
3. More robust logging - Background ILogger implementation to gather logging and give info on app health
4. Better error handling for tweet deserialization, bad connection, metric store connection
5. Metrics are simple counts - need a better scheme to handle more complex aggregation.  
 





