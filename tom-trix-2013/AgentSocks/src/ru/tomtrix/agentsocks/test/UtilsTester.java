package ru.tomtrix.agentsocks.test;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.StaxDriver;
import org.junit.*;

import java.io.*;
import java.util.Random;
import static org.junit.Assert.*;
import ru.tomtrix.agentsocks.infrastructure.LogicProcess;
import ru.tomtrix.agentsocks.infrastructure.Model;
import ru.tomtrix.agentsocks.infrastructure.Node;
import ru.tomtrix.agentsocks.mathmodel.Agent;
import ru.tomtrix.agentsocks.mathmodel.DefaultAgent;
import ru.tomtrix.agentsocks.utils.Collections;

/** dfase
 * @author tom-trix */
public class UtilsTester
{
	/** fsopjeio */
	private Random	_r	= new Random(System.currentTimeMillis());

	/** Test methods for <li>{@link ru.tomtrix.agentsocks.utils.Collections#toByteArray(int[])}<li>{@link ru.tomtrix.agentsocks.utils.Collections#toIntArray(byte[])}. */
	@Test
	public void testArrays()
	{
		final int tests = 20000;
		final int max = 2000;
		for (int i = 1; i <= tests; i++)
		{
			byte bytes[] = new byte[_r.nextInt(max) + 2];
			_r.nextBytes(bytes);
			int ints[] = Collections.toIntArray(bytes);
			byte nv[] = Collections.toByteArray(ints);
			assertArrayEquals(bytes, nv);
		}
	}

	/** Test method for {@link ru.tomtrix.agentsocks.utils.XMLSerializer}
	 * @throws Exception
	 */
	@Test
	public void testSerializer() throws Exception
	{
		// constants
		final int nodes = 20;
		final int processesPerNode = 20;
		final int agentsPerProcess = 20;

		// create model
		Model model = new Model("TestModel");
		for (int i = 0; i < nodes; i++)
		{
			model.addNode();
			Node node = model.getNodeByNumber(i);
			assertEquals(i, node.get_rank());
			for (int j = 0; j < processesPerNode; j++)
			{
				node.get_container().addLogicProcess("TestProcess_" + i + "_" + j);
				for (int k = 0; k < agentsPerProcess; k++)
				{
					LogicProcess process = node.get_container().getProcessByName("TestProcess_" + i + "_" + j);
					process.addAgent(new DefaultAgent("Agent_" + i + "_" + j + "_" + k, "Agent_" + i + "_" + j + "_" + k));
					Agent agent = process.getAgentByNumber(k);
					assertEquals(agent.get_name(), "Agent_" + i + "_" + j + "_" + k);
					agent.addVariable("public int x = " + _r.nextInt() + ";");
					agent.addVariable("long y = " + _r.nextLong() + ";");
					agent.addVariable("private String z = \"Test\";");
					agent.addFunction("public String getZ() {return z;}");
					agent.addFunction("void sayHey() {System.out.println(\"Hey!\");}");
					agent.addFunction("protected long getTime() {return System.currentTimeMillis();}");
					agent.addEvent(_r.nextDouble() * 100, "getZ");
					agent.addEvent(_r.nextDouble() * 100, "sayHey");
					agent.addEvent(_r.nextDouble() * 100, "getTime");
				}
			}
		}

        // serialize to file
        Writer writer = new FileWriter("~test");
        XStream xs = new XStream(new StaxDriver());
        xs.toXML(model, writer);
        writer.close();

        // deserialize
        Reader reader = new FileReader("~test");
        Model newModel = (Model)xs.fromXML(reader);
        reader.close();

        // delete the file
        if (!new File("~test").delete()) System.err.println("Error in IO");

		// assert what we've got
		assertEquals(nodes, newModel.getNodesCount());
		for (int i = 0; i < newModel.getNodesCount(); i++)
		{
			Node node = newModel.getNodeByNumber(i);
			for (int j = 0; j < processesPerNode; j++)
			{
				LogicProcess process = node.get_container().getProcessByName("TestProcess_" + i + "_" + j);
				for (int k = 0; k < agentsPerProcess; k++)
				{
					Agent agent = process.getAgentByName("Agent_" + i + "_" + j + "_" + k);
					assertEquals(agent, process.getAgentByNumber(k));
					agent.removeVariable("x");
					agent.removeVariable("y");
					assertEquals(1, agent.getState().size());
					assertTrue(agent.getState().contains("private String z = \"Test\";"));
					agent.removeFunction("getZ");
					agent.removeFunction("getTime");
					assertEquals(1, agent.getTransformFunctions().size());
					assertTrue(agent.getTransformFunctions().contains("void sayHey() {System.out.println(\"Hey!\");}"));
					assertNotNull(agent.getNextEventTime());
					agent.removeFunction("sayHey");
					assertNull(agent.getNextEventTime());
				}
			}
		}
	}
}
