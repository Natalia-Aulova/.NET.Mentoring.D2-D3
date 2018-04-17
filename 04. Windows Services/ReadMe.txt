1. Create a service which merges results of a streaming scanner into a single PDF file.
   The service should support the following:
   a) Monitor one or more folders on the disk;
   b) When images which names match for the <prefix>_<number>.<png|jpeg|bmp> template appear in the folders 
      it should collect the sequence of pages and save them as a single document into a separate folder;
   c) When it starts it should scan the folders and merge found files into a document if they exist.
   As an "end of the document" it is proposed to use the following:
   a) "Jumping" numbering of documents 
      (e.g. img_001.jpeg, img_002.jpeg, img_004.jpeg means 2 documents containing 2 and 1 pages respectively);
   b) The next page has timed out.
   Notes:
   a) Documents should be saved in PDF format;
   b) The service should be implemented based on Topshelf.

2. Be sure that the service processes the following situations correctly:
   a) A source file which should be processed is used by other process 
     (possible solution: try several times to open and only after that an error should be thrown);
   b) A command was issued to stop the service during a long operation (e.g. creating multipage file from a large count of pages);
   c) One or more pages of multipage file are broken (e.g. wrong format).
      Possible solution: moving a whole sequence of pages to a folder for broken files.
	  A broken file can be created by changing an extension of a file which is not an image.

Implementation notes:
	To run the following commands go to a folder which contains .exe file of the service and open CMD.
	1) To install the service:
	   WindowsService.FileHandler.exe install --sudo
	2) To uninstall the service:
	   WindowsService.FileHandler.exe uninstall --sudo
	3) To start the service:
	   WindowsService.FileHandler.exe start
	4) To stop the service:
	   WindowsService.FileHandler.exe stop
	5) To get help for other commands of the service:
	   WindowsService.FileHandler.exe --help