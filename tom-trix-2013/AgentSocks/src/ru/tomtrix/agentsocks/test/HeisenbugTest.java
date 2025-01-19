package ru.tomtrix.agentsocks.test;

import org.junit.Test;
import java.util.Random;
import static org.junit.Assert.*;
import ru.tomtrix.agentsocks.mathmodel.*;
import ru.tomtrix.agentsocks.infrastructure.*;
import ru.tomtrix.javassistwraper.ClassStore;
import ru.tomtrix.agentsocks.messaging.LocalMail;

/** gsrrg
 * @author tom-trix */
public class HeisenbugTest
{
	/** hasdiofoied */
	private Random	_r	= new Random(System.currentTimeMillis());

	/** fsnifnjeo
	 * @throws Exception */
	@Test
	public void test() throws Exception
	{
		//constants
		final int threads = 3;
		final int msgs = 100;
		final int sleep = 5000;
		
		//load necessary classes to Javassist
		ClassStore.getInstance().addClassPath(LocalMail.class);
		ClassStore.getInstance().addImport(LocalMail.class.getPackage().getName());
		
		//create simple debug model that will consist of 3 processes residing within 1 node
		Model model = new Model("debug");
		model.addNode();
		Node node = model.getNodeByNumber(0);
		for (int i = 0; i < threads; i++)
		{
			node.get_container().addLogicProcess("Process" + i);
			LogicProcess p = node.get_container().getProcessByName("Process" + i);
			p.addAgent(new DefaultAgent("TestAgent" + i, "TestAgent" + i));
			Agent agent = p.getAgentByNumber(0);
			agent.addVariable("public long msg = 0L;");
			agent.addFunction(String.format("public void go() {msg++; LocalMail.send(new StateChanged(\"msg\", Long.valueOf(msg)), \"Process%d\", \"TestAgent%d\", \"TestAgent%d\");}", (i + 1) % threads, (i + 1) % threads, i));
			for (int j = 0; j < msgs; j++)
				agent.addEvent(_r.nextDouble() * 100, "go");
		}
		
		//compile the model
		model.compileAgents();
		
		//run it!
		node.run();
		Thread.sleep(sleep);
		assertTrue(true);
	}
}
