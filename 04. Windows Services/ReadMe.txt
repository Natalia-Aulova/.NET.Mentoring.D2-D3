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
