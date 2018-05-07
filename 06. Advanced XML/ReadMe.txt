1. Checking an xml file
2. RSS Feed

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