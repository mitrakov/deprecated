package ru.tomtrix.agentsocks.modeleditor;

import java.util.Arrays;
import ru.tomtrix.consoleui.*;
import org.apache.log4j.Logger;
import ru.tomtrix.agentsocks.mathmodel.*;
import ru.tomtrix.productions.VariableType;
import ru.tomtrix.agentsocks.infrastructure.*;
import ru.tomtrix.agentsocks.utils.XMLSerializer;

/** jfsofeos
 * @author tom-trix */
public class MVCModel implements ConsoleUIListener
{
	private static final String	ERROR_STR	= "Error occured";

	/** fnsefnieo */
	private ConsoleUI			_cuiRef;
    /** fshisfueih */
	private Model				_model;
	/** jdfaiofsoei */
	private ProcessAndAgent		_pa;

	/** fsemklfel
	 * @author tom-trix */
	private class ProcessAndAgent
	{
		public LogicProcess	process;
		public Agent		agent;

		public ProcessAndAgent(LogicProcess p, Agent a)
		{
			process = p;
			agent = a;
		}
	}

	/** fshirfiiu
	 * @param name fse
	 * @return hg */
	public String createModel(String name)
	{
		try
		{
			_model = new Model(name.trim());
			return String.format("OK. Model \"%s\" has been created", name);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fsejofhbeui
	 * @param filename fwe
	 * @return fews */
	public String loadModel(String filename)
	{
		try
		{
			_model = new XMLSerializer<Model>().deserializeFromFile(filename);
			_model.loadCode();
			return String.format("OK. Model \"%s\" has been loaded", _model.get_name());
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** dfahifehui
	 * @param filename fe
	 * @return grd */
	public String saveModel(String filename)
	{
		try
		{
			new XMLSerializer<Model>().serializeToFile(_model, filename);
			return String.format("OK. Model \"%s\" has been saved", _model.get_name());
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fhisufsei
	 * @return grss */
	public String showModel()
	{
		return _model.toString();
	}

	/** dfaseihfi
	 * @return gredg */
	public String createNode()
	{
		try
		{
			return String.format("OK. Node %d has been created", _model.addNode());
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** @return fshoehfeuio */
	public String deleteLastNode()
	{
		try
		{
			_model.deleteLastNode();
			return String.format("OK. Node and all its logic processes have been removed. There are %d nodes left", _model.getNodesCount());
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fshjoieio
	 * @param name fggt
	 * @param rank geds
	 * @return gde */
	public String createProcess(String name, String rank)
	{
		try
		{
			_model.getNodeByNumber(Integer.parseInt(rank)).get_container().addLogicProcess(name.trim());
			return String.format("OK. Logic process \"%s\" has been created on node %s", name, rank);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** sfeohis
	 * @param name hrt
	 * @param rank srf
	 * @param newname gr
	 * @return gde */
	public String renameProcess(String name, String rank, String newname)
	{
		try
		{
			_model.getNodeByNumber(Integer.parseInt(rank)).get_container().getProcessByName(name.trim()).set_name(newname.trim());
			return String.format("OK. Logic process \"%s\" has been renamed into \"%s\"", name, newname);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** afjoeefoi
	 * @param name gd
	 * @param rank hrft
	 * @param newRank dwa
	 * @return gdr */
	public String moveProcess(String name, String rank, String newRank)
	{
		try
		{
			Container c = _model.getNodeByNumber(Integer.parseInt(rank)).get_container();
			LogicProcess lp = c.getProcessByName(name);
			c.removeProcess(name);
			_model.getNodeByNumber(Integer.parseInt(newRank)).get_container().addLogicProcess(lp);
			return String.format("OK. Logic process \"%s\" has been moved from %s to %s", name, rank, newRank);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fsohjoef
	 * @param name grsd
	 * @param rank gsr
	 * @return rg */
	public String deleteProcess(String name, String rank)
	{
		try
		{
			_model.getNodeByNumber(Integer.parseInt(rank)).get_container().removeProcess(name);
			return String.format("OK. Logic process \"%s\" and all its agents have been removed from the node %s", name, rank);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** uafopjafo
	 * @param name frsegr
	 * @param process gdth
	 * @param rank frg
	 * @return efs */
	public String createDefaultAgent(String name, String process, String rank)
	{
		try
		{
            return createAgent(name, process, rank, true);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

    /** uafopjafo
     * @param name frsegr
     * @param process gdth
     * @param rank frg
     * @return efs */
    public String createProductionAgent(String name, String process, String rank)
    {
        try
        {
            return createAgent(name, process, rank, false);
        }
        catch (Exception e)
        {
            Logger.getLogger(getClass()).error(ERROR_STR, e);
            return e.toString();
        }
    }

	/** faseoifeo
	 * @param name grse
	 * @param process gr
	 * @param rank gdrt
	 * @return fse */
	public String useAgent(String name, String process, String rank)
	{
		try
		{
			if (_pa != null && _pa.agent != null && _cuiRef != null) _cuiRef.pop_greeting();
			LogicProcess p = _model.getNodeByNumber(Integer.parseInt(rank)).get_container().getProcessByName(process);
			_pa = new ProcessAndAgent(p, p.getAgentByName(name));
			if (_cuiRef != null) _cuiRef.push_greeting(name);
			return String.format("OK. Now agent \"%s\" is currently used", name);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fsvonedpa
	 * @param newname hrf
	 * @return efs */
	public String renameAgent(String newname)
	{
		try
		{
			_pa.agent.set_name(newname.trim());
            if (_cuiRef != null)
            {
                _cuiRef.pop_greeting();
                _cuiRef.push_greeting(newname);
            }
			return String.format("OK. Agent has been renamed into \"%s\"", newname);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fasejoijho
	 * @param newProcess gdrgt
	 * @param newNode ger
	 * @return gre */
	public String moveAgent(String newProcess, String newNode)
	{
		try
		{
			_pa.process.removeAgent(_pa.agent);
			_pa.process = _model.getNodeByNumber(Integer.parseInt(newNode)).get_container().getProcessByName(newProcess);
			_pa.process.addAgent(_pa.agent);
			return String.format("OK. Agent \"%s\" has been moved to logic process \"%s\" on node %s", _pa.agent.get_name(), newProcess, newNode);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fasjoijefo
	 * @return efs */
	public String deleteAgent()
	{
		try
		{
			String s = String.format("OK. Agent \"%s\" has been removed from process \"%s\"", _pa.agent.get_name(), _pa.process.get_name());
			_pa.process.removeAgent(_pa.agent);
			_pa = null;
			if (_cuiRef != null) _cuiRef.pop_greeting();
			return s;
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** fsiefi
	 * @param code fse
	 * @return rs */
	public String addVariable(String code)
	{
		try
		{
			_pa.agent.addVariable(code);
			return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

    /**
     * fgregthhyr
     * @param code gferge
     * @param type greh
     * @return vgnsn
     */
    public String addVariable(String code, String type)
    {
        try
        {
            ((ProductionAgent)_pa.agent).addVariable(code, VariableType.valueOf(type));
            return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
        }
        catch (Exception e)
        {
            Logger.getLogger(getClass()).error(ERROR_STR, e);
            return e.toString();
        }
    }

	/** ohfsiohabif
	 * @param var fsefs
	 * @return few */
	public String deleteVariable(String var)
	{
		try
		{
			_pa.agent.removeVariable(var);
			return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** joefai
	 * @param code fgrse
	 * @return f */
	public String addFunction(String code)
	{
		try
		{
			_pa.agent.addFunction(code);
			return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** pkfoeshui
	 * @param fid grse
	 * @return fws */
	public String deleteFunction(String fid)
	{
		try
		{
			_pa.agent.removeFunction(fid);
			return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** @param fid fsegr
	 * @param timestamp fg
	 * @param params ghrt
	 * @return fws */
	public String addEvent(String fid, String timestamp, String params)
	{
		try
		{
			_pa.agent.addEvent(Double.parseDouble(timestamp), fid);
			return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

	/** posijoefjo
	 * @param fid htr
	 * @param timestamp fse
	 * @return fse */
	public String deleteEvent(String fid, String timestamp)
	{
		try
		{
			_pa.agent.removeEvent(Double.parseDouble(timestamp));
			return String.format("OK. Agent \"%s\" has got the definition changed:\n%s", _pa.agent.get_name(), _pa.agent);
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error(ERROR_STR, e);
			return e.toString();
		}
	}

    /** hcfseaio */
    public String exit()
    {
        if (_cuiRef != null) _cuiRef.stop();
        return "Bye!";
    }

    public String createAgent(String name, String process, String rank, boolean isDefault) throws Exception
    {
        // push greeting
        if (_pa != null && _pa.agent != null && _cuiRef != null) _cuiRef.pop_greeting();
        if (_cuiRef != null)
            _cuiRef.push_greeting(name);
        // create new agent and "_pa" instance
        _pa = new ProcessAndAgent(_model.getNodeByNumber(Integer.parseInt(rank.trim())).get_container().getProcessByName(process.trim()), isDefault ? new DefaultAgent(name.trim(), name.trim()) : new ProductionAgent(name.trim(), name.trim()));
        _pa.process.addAgent(_pa.agent);
        //return
        return String.format("OK. Agent \"%s\" has been created.\nNow it is currently used", name);
    }

    public String getFunctionByFid(String fid)
    {
        for (String s : _pa.agent.getTransformFunctions())
            if (Arrays.asList(s.split("[( ]")).contains(fid))   //TODO
                return s;
        return null;
    }

    public String getVariableDefinition(String var)
    {
        for (String s : _pa.agent.getState())
            if (Arrays.asList(s.split("[;= ]")).contains(var)) //TODO
                return s;
        return null;
    }

    public Agent getCurrentAgent()
    {
        if (_pa == null) return null;
        return _pa.agent;
    }

	@Override
	public void setConsoleUI(ConsoleUI cui)
	{
		_cuiRef = cui;
	}

    /** @return model */
    public Model get_model() {
        return _model;
    }
}
