Task 1
The following method is used to generate a hash-password:

public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
{
	var iterate = 10000;
	var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
	byte[] hash = pbkdf2.GetBytes(20);
	byte[] hashBytes = new byte[36];
	Array.Copy(salt, 0, hashBytes, 0, 16);
	Array.Copy(hash, 0, hashBytes, 16, 20);
	var passwordHash = Convert.ToBase64String(hashBytes);
	return passwordHash;
}

Try to speed up the method without reducing the number of iterations contained in the iterate variable.
To measure the time of the method use the Stopwatch class and several method runs 
then divide the obtained time by the number of runs.

Task 2
Analyze the ASP.NET MVC application from the ProfileSample.zip archive. 
Optimize the algorithm for loading images from the database and displaying them. 
The database backup is located in the App_Data folder.

Task 3
The DumpHomework.zip archive contains an application which caused a user error. 
A dump file is attached to the application. Determine the error and correct it if possible.

Task 4
Optimize the application from the GameOfLife.zip archive both in terms of memory leaks and performance.