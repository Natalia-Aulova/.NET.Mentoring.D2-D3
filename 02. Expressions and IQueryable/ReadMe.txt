1. IQueryable
	Modify the LINQ provider presented in the lecture.
	In particular, you need to add the following:
		a) Remove the current restriction on the order of the operands in the expression. Should be allowed:
			- <name of the filtered field> == <constant> (only this one is available now)
			- <constant> == <name of the filtered field>
		b) Add support for inclusion operations (ie, not exact match with a string, but partial one). 
		   In LINQ notation they should look like the following method calls of class string: 
				- StartsWith (Where(e => e.workstation.StartsWith("EPRUIZHW006")) -> workstation:(EPRUIZHW006*))
				- EndsWith (Where(e => e.workstation.EndsWith("IZHW0060")) -> workstation:(*IZHW0060))
				- Contains (Where(e => e.workstation.Contains("IZHW006")) -> workstation:(*IZHW006*))
		c) Add support for the AND operator (might require modifying the E3SQueryClient class). 
		   The organization of the AND operator in the query to E3S can be found on the documentation page 
		   (https://kb.epam.com/display/E3S/E3S+public+REST+for+data: section FTS Request Syntax)

2. Expression Tree
	Using the ability to design an Expression Tree and execute its code, 
	create your own mapping mechanism (copy fields/properties of one class to another one).