1.	Write a program, which creates an array of 100 Tasks, runs them and wait all of them are not finished. 
	Each Task should iterate from 1 to 1000 and print into the console the following string: "Task #0 - {iteration number}".
2.	Write a program, which creates a chain of four Tasks. 
	First Task - creates an array of 10 random integer. 
	Second Task - multiplies this array with another random integer. 
	Third Task - sorts this array by ascending. 
	Fourth Task - calculates the average value. 
	All these tasks should print the values to console.
3.	Write a program, which multiplies two matrices and uses class Parallel. 
4.	Write a program which recursively creates 10 threads. 
	Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread. 
	Use Thread class for this task and Join for waiting threads.
5.	Write a program which recursively creates 10 threads. 
	Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread. 
	Use ThreadPool class for this task and Semaphore for waiting threads.
6.	Write a program which creates two threads and a shared collection: 
	the first one should add 10 elements into the collection, 
	the second one should print all elements in the collection after each adding. 
	Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
7.	Create a Task and attach continuations to it according to the following criteria:
	a.	Continuation task should be executed regardless of the result of the parent task.
	b.	Continuation task should be executed when the parent task finished without success.
	c.	Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
	d.	Continuation task should be executed outside of the thread pool when the parent task would be cancelled
Demonstrate the work of the each case with console utility.
