package ru.tomtrix.mpiagent.tester;

import mpi.MPI;
import ru.tomtrix.mpiagent.MPIAgent;

/** Example of using MPIAgent. Run it through an MPI-launcher (e.g. for Linux: <b>$MPJ_HOME/bin/mpjrun.sh -np 2 -jar mpiagent.jar</b> or for Windows: <b>%MPJ_HOME%\bin\mpjrun.exe -np 2 -jar mpiagent.jar</b>)
 * @author tom-trix */
public class Starter
{
	public static void main(String[] args)
	{
		try
		{
			MPI.Init(args);
			MPIAgent.getInstance().start(new Tester(), 100);
			MPI.Finalize();
		}
		catch (Exception e)
		{
			e.printStackTrace();
		}
	}
}
