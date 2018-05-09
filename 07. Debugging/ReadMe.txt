Using the file CrackMe.exe which checks correctness of the entered code 
and using the debugging techniques find the correct code and write a code generator.

Notes:

1) To get IL code of the .exe file:
ildasm /out="<path_to_output_file.il>" "<path_to_input_file.exe>"

2) To get .exe file in the debug mode from IL code:
ilasm /exe /pdb /debug "<path_to_input_file.il>"
