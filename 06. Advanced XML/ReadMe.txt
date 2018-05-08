1. Checking an xml file
   - Create XML Schema for an XML file (see the structure of books.xml). Implement the following checks:
	 a) The isbn tag is optional; when it is presented it should have the correct structure.
	 b) The genre tag should contain any value from the predefined list (see possible values in books.xml).
	 c) The publish_date and registration_date tags should be in the format yyyy-mm-dd.
	 d) The id attribute should be unique per a file.
   - Implement an XML validator which uses the schema above, checks an input file and provides information about errors
     (e.g. the line and position of a tag with an error).

2. RSS Feed
   - Implement XSL transformation from the format of books.xml to the RSS format.
	 a) Use a registration date as a date of news
	 b) If a book has the Science Fiction genre and the isbn tag is presented then use 
	    http://my.safaribooksonline.com/<isbn>/ as a link to full news.
   - Implement an XML transformer which receives an input file in the format of books.xml and
     creates new RSS feed.

Development Notes

Regular expression used for validating ISBN and its description:
^                         # Binding to the beginning of a string.
(?:ISBN(?:-1[03])?:?\ )?  # Optional ISBN/ISBN-10/ISBN-13 identifier.
(?=                       # Basic format pre-checks (lookahead):
  [\dX]{10}$              #   Require 10 digits/Xs (no separators).
 |                        #  Or:
  (?=(?:\d+[-\ ]){3})     #   Require 3 separators
  [-\ \dX]{13}$           #     out of 13 characters total.
 |                        #  Or:
  97[89]\d{10}$           #   978/979 plus 10 digits (13 total).
 |                        #  Or:
  (?=(?:\d+[-\ ]){4})     #   Require 4 separators
  [-\ \d]{17}$            #     out of 17 characters total.
)                         # End format pre-checks.
(?:97[89][-\ ]?)?         # Optional ISBN-13 prefix.
\d{1,5}[-\ ]?             # 1-5 digit group identifier.
\d+[-\ ]?\d+[-\ ]?        # Publisher and title identifiers.
[\dX]                     # Check digit.
$                         # Binding to the end of a string.