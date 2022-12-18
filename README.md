# jh-code-challenge
An app to stream from Twitter API sample Tweet stream. 

The purpose of the app is to demonstrate technique and organization in how I write and organize code . Its function is:

1. Utilize the sample stream endpoint provided in the Twitter API.
2. Track Statistics on the recieved tweets. 
    
    **Required**
    * Total count of Tweets
    * Top 10 Hashtags
    
    **Potential**
    * Average number of Tweets in a given timeframe
    * Top sources
    * Top langauges

3. Provide a means of reporting the gathered statitics to the user

## Running the Program



## Design Considerations
1. The app should not block statists reporting while processing tweets. (And vice versa.)

2. Demonstrate SOLID principles in its design, and be loosly couple to any external dependencies (Twitter API, datastore, etc.)
 
3. Demonstrate Error Handling and Unit Testing

4. Demostrate logging (Not explicitly stated in the requirements doc, but "production ready" code should have this.)

There is no requirement to store the Tweets recived in a database or other durable repository. But thought must be given as to how this could be accomplished.

*Code should be considered production ready.*

## Initial Design

### Description
The application will be a dotnet console applicaiton, that will run the stream component and the processing component in parallel.  The console will provide an on demand reporting  mechanism as well as an option to periodically refresh automatically.

There will be three main components to the application: A Twitter stream service, a Tweet processing service, and User interface to provide the reporting features.

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
Thought on this is deferred for the time being.

The following documents are provided to demonstrate the thought process that went into creating the apps over all design and may not reflect the state of end product.  

* [Component Diagram](https://lucid.app/lucidchart/de91193b-9a64-4f41-880e-f92d720cc386/edit?viewport_loc=-95%2C-31%2C1997%2C919%2C0_0&invitationId=inv_b96b4710-0691-496f-80e5-16a467195f8a)

