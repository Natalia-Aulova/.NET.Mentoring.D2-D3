1. Centralizing collection of input services results
   Implement a central service that will do the following:
   a) When installing (or first starting) creates a queue for receiving results (ready-made documents) from the input services;
   b) Listens to this incoming queue and saves all incoming documents to disk.
   
   Modify the input service so that it sends ready-made documents through the messaging service 
   to the central server instead of saving them to disk.
   To simplify the architecture we assume that several agents (on different computers) and only 1 central service
   can work simultaneously in the network.

   Notes:
   - Input services will send links to local files;
   - Central service will download these files using the links.

2. Centralized control and management
   Implement a mechanism of centralized management through message queues.
   The following should be done:
   a) The current status of the input service should come to the central server with some frequency:
      - What the service does (wait for new files/process a sequence/etc);
	  - Timeout to wait for the next page.
   b) Commands are sent from the central service to input services:
      - Update a status (not waiting for the status update time);
	  - Change timeout to wait for the next page.

   Notes:
   - The central service should save statuses of input services to CSV file;
   - Monitor a local file with settings to send new ones if the file is updated.
