package ru.tomtrix.agentsocks.modeleditor;

import java.util.*;
import javax.swing.*;
import java.awt.event.*;
import javax.swing.event.*;
import java.util.Collection;
import ru.tomtrix.productions.*;
import javax.swing.tree.TreeNode;
import static javax.swing.JOptionPane.*;
import ru.tomtrix.agentsocks.mathmodel.*;
import com.sun.jmx.remote.internal.ArrayQueue;
import static ru.tomtrix.productions.VariableType.*;

/** fwer */
public class GUIController implements MouseListener, TreeSelectionListener, ListSelectionListener
{
    public final transient static String SPLIT = " ";
    public final transient static String SPLIT_COLON = ": ";

    private final MVCModel _mvcModelRef;
    private final GraphicUI _mvcViewRef;
    /** Переиспользуемая ссылка на popup-меню */
    private JPopupMenu _menu;
    /** Переиспользуемая ссылка на пункт popup-меню */
    private JMenuItem _item;

    public GUIController(MVCModel model, GraphicUI view)
    {
        if (model==null || view == null) throw new NullPointerException("fsr");
        _mvcModelRef = model;
        _mvcViewRef = view;
    }

    private JPopupMenu getTreeMenu()
    {
        final TreeNode cur = (TreeNode) _mvcViewRef.getTree().getLastSelectedPathComponent();
        if (cur == null) return null;
        _menu = new JPopupMenu();
        if (cur.toString().startsWith("<"))
        {
            _item = new JMenuItem("Create new Model");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String s = showInputDialog(null, "Input the model name");
                    if (s == null || s.trim().isEmpty()) return;
                    _mvcViewRef.setStatus(_mvcModelRef.createModel(s.trim()));
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
        }
        else if (cur.toString().startsWith("Model"))
        {
            _item = new JMenuItem("Create new Node");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    _mvcViewRef.setStatus(_mvcModelRef.createNode());
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
        }
        else if (cur.toString().startsWith("Node"))
        {
            final String rank = cur.toString().split(SPLIT)[1];
            _item = new JMenuItem("Create new Logic Process");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String s = showInputDialog(null, "Input the process name");
                    if (s == null || s.trim().isEmpty()) return;
                    _mvcViewRef.setStatus(_mvcModelRef.createProcess(s.trim(), rank));
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
        }
        else if (cur.toString().startsWith("Process"))
        {
            final String name = cur.toString().split(SPLIT)[1];
            final String rank = cur.getParent().toString().split(SPLIT)[1];
            _item = new JMenuItem("Create new DefaultAgent");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String s = showInputDialog(null, "Input the agent name");
                    if (s==null || s.trim().isEmpty()) return;
                    _mvcViewRef.setStatus( _mvcModelRef.createDefaultAgent(s, name, rank));
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
            _item = new JMenuItem("Create new ProductionAgent");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String s = showInputDialog(null, "Input the agent name");
                    if (s==null || s.trim().isEmpty()) return;
                    _mvcViewRef.setStatus( _mvcModelRef.createProductionAgent(s, name, rank));
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
            _item = new JMenuItem("Rename process");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String s = showInputDialog(null, "Input new process name");
                    if (s==null || s.trim().isEmpty()) return;
                    _mvcViewRef.setStatus(_mvcModelRef.renameProcess(name, rank, s));
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
            _item = new JMenuItem("Delete process");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    if (showConfirmDialog(null, "Are you sure?") != YES_OPTION) return;
                    _mvcViewRef.setStatus(_mvcModelRef.deleteProcess(name, rank));
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
        }
        else if (cur.toString().startsWith("Agent"))
        {
            Agent agent = _mvcModelRef.getCurrentAgent();
            if (agent==null) return null;
            _item = new JMenuItem("Rename agent");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String newname = showInputDialog(null, "Input a new name");
                    if (newname==null || newname.trim().isEmpty()) return;
                    _mvcModelRef.renameAgent(newname);
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
            _item = new JMenuItem("Delete agent");
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    if (showConfirmDialog(null, "Are you sure?") != YES_OPTION) return;
                    _mvcModelRef.deleteAgent();
                    _mvcViewRef.rebuildTreeByModel();
                }
            });
            _menu.add(_item);
            if (agent instanceof ProductionAgent)
            {
                final ProductionAgent agnt = (ProductionAgent)agent;
                _item = new JMenuItem("Manage rules");
                _item.addActionListener(new AbstractAction() {
                    @Override
                    public void actionPerformed(ActionEvent e) {
                        _mvcViewRef.refreshRules(agnt.get_ruleTexts());
                        JDialog dialog = new JDialog(_mvcViewRef, "Rules for " + agnt.get_name(), true);
                        dialog.setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
                        dialog.setSize(700, 300);
                        dialog.setLocationRelativeTo(null);
                        dialog.getContentPane().add(new JScrollPane(_mvcViewRef.getRulesTextbox()));
                        dialog.setVisible(true);
                    }
                });
                _menu.add(_item);
            }
        }
        return _menu;
    }

    private Agent getAgent()
    {
        Agent agent = _mvcModelRef.getCurrentAgent();
        if (agent==null)
        {
            showMessageDialog(null, "Select an agent from the Model Structure", "Error", ERROR_MESSAGE);
            return null;
        }
        return agent;
    }

    private JPopupMenu getVariablesMenu()
    {
        final Agent agent = getAgent();
        if (agent == null) return null;
        final String cur =  _mvcViewRef.getVariablesListbox().getSelectedValue().toString();
        _menu = new JPopupMenu();
        _item = new JMenuItem("Add variable to " + agent.get_name());
        _item.addActionListener(new AbstractAction() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String s = showInputDialog(null, "Input the variable definition");
                if (s == null || s.trim().isEmpty()) return;
                s = _mvcModelRef.addVariable(s);
                if (s.contains("Exception"))
                {
                    showMessageDialog(null, "Input a correct variable, e.g. \"public int x = 8;\"", "Error", ERROR_MESSAGE);
                    _mvcViewRef.setStatus(s);
                }
                else _mvcViewRef.setStatus("Everything is OK");
                _mvcViewRef.refreshVariables(agent.getVariables());
            }
        });
        _menu.add(_item);
        if (agent instanceof ProductionAgent)
        {
            _item = new JMenuItem("Add production variable to " + agent.get_name());
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    String s = showInputDialog(null, "Input the variable name");
                    if (s == null || s.trim().isEmpty()) return;
                    int result = showOptionDialog(null, "Which type would you want to assign?", "Choose the type", YES_NO_CANCEL_OPTION, QUESTION_MESSAGE, null, new String[]{"INFERRIBLE", "INFERRIBLE-ASKABLE", "ASKABLE"}, null);
                    s = _mvcModelRef.addVariable(s, (result==YES_OPTION ? INFERRIBLE : result==NO_OPTION ? INFERRIBLY_ASKABLE : ASKABLE).toString());
                    if (s.contains("Exception"))
                    {
                        showMessageDialog(null, "Input a correct variable, e.g. \"public int x = 8;\"", "Error", ERROR_MESSAGE);
                        _mvcViewRef.setStatus(s);
                    }
                    else _mvcViewRef.setStatus("Everything is OK");
                    _mvcViewRef.refreshVariables(agent.getVariables());
                }
            });
            _menu.add(_item);
            if (cur.startsWith("_") && !cur.equals("_core") && !cur.equals("_ruleset"))
            {
                _item = new JMenuItem("Initialize production variable");
                _item.addActionListener(new ActionListener() {
                    @Override
                    public void actionPerformed(ActionEvent e) {
                        String s = showInputDialog(null, "Input the value");
                        if (s==null || s.trim().isEmpty()) return;
                        ((ProductionAgent) agent).initializeVariable(cur.substring(1), s);
                    }
                });
            }
            _menu.add(_item);
        }
        if (!cur.startsWith("<") && !cur.equals("_core") && !cur.equals("_ruleset"))
        {
            _item = new JMenuItem("Delete variable from " + agent.get_name());
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    if (showConfirmDialog(null, "Are you sure?") != YES_OPTION) return;
                    _mvcViewRef.setStatus(_mvcModelRef.deleteVariable(cur));
                    _mvcViewRef.refreshVariables(agent.getVariables());
                }
            });
            _menu.add(_item);
        }
        return _menu;
    }

    private JPopupMenu getEventsMenu()
    {
        final Agent agent = getAgent();
        if (agent == null) return null;
        final String cur =  _mvcViewRef.getEventsListbox().getSelectedValue();
        _menu = new JPopupMenu();
        _item = new JMenuItem("Add event to " + agent.get_name());
        _item.addActionListener(new AbstractAction() {
            @Override
            public void actionPerformed(ActionEvent e) {
                Collection<String> fids = agent.getFids();
                if (fids.isEmpty()) {
                    showMessageDialog(null, "There are no functions. Add a function first", "Error", ERROR_MESSAGE);
                    return;
                }
                Object fid = showInputDialog(null, "Choose the function name", "Choose the function", PLAIN_MESSAGE, null, fids.toArray(), null);
                if (fid == null || fid.toString().trim().isEmpty()) return;
                String time = showInputDialog(null, "Input the timestamp");
                if (time == null || time.trim().isEmpty()) return;
                String s = _mvcModelRef.addEvent(fid.toString(), time, null);
                if (!s.contains("Exception"))
                    _mvcViewRef.setStatus("Everything is OK");
                _mvcViewRef.refreshEvents(agent.getEvents());
            }
        });
        _menu.add(_item);
        if (!cur.startsWith("<") && !cur.equals("0.00: initialize"))
        {
            _item = new JMenuItem("Delete event from " + agent.get_name());
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    if (showConfirmDialog(null, "Are you sure?") != YES_OPTION) return;
                    _mvcViewRef.setStatus(_mvcModelRef.deleteEvent(cur.split(SPLIT_COLON)[1], cur.split(SPLIT_COLON)[0]));
                    _mvcViewRef.refreshEvents(agent.getEvents());
                }
            });
            _menu.add(_item);
        }
        return _menu;
    }

    private JPopupMenu getFidsMenu()
    {
        final Agent agent = getAgent();
        if (agent == null) return null;
        if (_mvcViewRef.getFunctionText().trim().isEmpty())
        {
            showMessageDialog(null, "Please, input the function code below! E.g.\n\npublic void go() {\n   ...\n}", "Error", ERROR_MESSAGE);
            return null;
        }
        final String cur =  _mvcViewRef.getFidsListbox().getSelectedValue();
        _menu = new JPopupMenu();
        _item = new JMenuItem("Add function to " + agent.get_name());
        _item.addActionListener(new AbstractAction() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String s = _mvcModelRef.addFunction(_mvcViewRef.getFunctionText());
                if (s.contains("Exception"))
                    showMessageDialog(null, s, "Error", ERROR_MESSAGE);
                else
                {
                    _mvcViewRef.setStatus("Everything is OK");
                    _mvcViewRef.refreshFids(agent.getFids());
                }
            }
        });
        _menu.add(_item);
        if (!cur.startsWith("<"))
        {
            _item = new JMenuItem("Save function changes in " + agent.get_name());
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    showMessageDialog(null, "Not implemented yet");
                    _mvcViewRef.refreshFids(agent.getFids());
                }
            });
            _menu.add(_item);
            _item = new JMenuItem("Delete function from " + agent.get_name());
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    if (showConfirmDialog(null, "Are you sure?") != YES_OPTION) return;
                    _mvcViewRef.setStatus(_mvcModelRef.deleteFunction(cur));
                    _mvcViewRef.refreshFids(agent.getFids());
                    _mvcViewRef.refreshEvents(agent.getEvents());
                }
            });
            _menu.add(_item);
        }
        return _menu;
    }

    private JPopupMenu getRulesMenu()
    {
        final ProductionAgent agent = (ProductionAgent) getAgent();
        final String cur = _mvcViewRef.getRulesTextbox().getSelectedValue();
        _menu = new JPopupMenu();
        _item = new JMenuItem("Add rule to " + agent.get_name());
        _item.addActionListener(new AbstractAction() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String name = showInputDialog(null, "Input the rule name (e.g. \"R1\")");
                if (name == null) return;
                if (agent.ruleExists(name))
                {
                    showMessageDialog(null, "This name already exists", "Error", ERROR_MESSAGE);
                    return;
                }
                List<String> variables = new ArrayQueue<>(16);
                List<Inequality> signs = new ArrayQueue<>(16);
                List<String> values = new ArrayQueue<>(16);
                List<Operation> operations = new ArrayQueue<>(15);
                while (true)
                {
                    String s = showInputDialog(null, "Input a variable");
                    if (s == null) return;
                    variables.add(s);
                    Inequality d = (Inequality)showInputDialog(null, "Choose EQUALS or NOT_EQUALS", "", QUESTION_MESSAGE, null, new Inequality[] {Inequality.EQUALS, Inequality.NOT_EQUALS}, Inequality.EQUALS);
                    if (d == null) return;
                    signs.add(d);
                    s = showInputDialog(null, "Input value");
                    if (s == null) return;
                    values.add(s);
                    int option = showOptionDialog(null, "Now you gotta concatinate the condition or say \"Finish\"", "", YES_NO_CANCEL_OPTION, PLAIN_MESSAGE, null, new Object[]{"AND", "OR", "Finish"}, null);
                    if (option==0)
                        operations.add(Operation.AND);
                    else if (option==1)
                        operations.add(Operation.OR);
                    else break;
                }
                String then1 = showInputDialog(null, "Input the result variable (THEN-clause)");
                if (then1 == null) return;
                String then2 = showInputDialog(null, "Input the result value (THEN-clause)");
                if (then2 == null) return;
                agent.addRule(name, variables, signs, values, operations, then1, then2);
                _mvcViewRef.refreshRules(agent.get_ruleTexts());
            }
        });
        _menu.add(_item);
        if (!cur.startsWith("<"))
        {
            _item = new JMenuItem("Delete rule from " + agent.get_name());
            _item.addActionListener(new AbstractAction() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    agent.removeRule(cur.split(SPLIT_COLON)[0]);
                    _mvcViewRef.refreshRules(agent.get_ruleTexts());
                }
            });
            _menu.add(_item);
        }
        return _menu;
    }

    @Override
    public void mousePressed(MouseEvent e)
    {
        if (!(e.getSource() instanceof JComponent)) return;
        if (!SwingUtilities.isRightMouseButton(e)) return;
        JPopupMenu m;
        switch (((JComponent)e.getSource()).getName())
        {
            case "tree":
                m = getTreeMenu();
                if (m!=null)
                    _menu.show(_mvcViewRef.getTree(), e.getX(), e.getY());
                break;
            case "variables":
                m = getVariablesMenu();
                if (m!=null)
                    _menu.show(_mvcViewRef.getVariablesListbox(), e.getX(), e.getY());
                break;
            case "events":
                m = getEventsMenu();
                if (m!=null)
                    _menu.show(_mvcViewRef.getEventsListbox(), e.getX(), e.getY());
                break;
            case "fids":
                m = getFidsMenu();
                if (m!=null)
                    _menu.show(_mvcViewRef.getFidsListbox(), e.getX(), e.getY());
                break;
            case "rules":
                m = getRulesMenu();
                if (m!=null)
                    _menu.show(_mvcViewRef.getRulesTextbox(), e.getX(), e.getY());
                break;
            default: throw new NoSuchElementException("gtd");
        }
    }

    @Override
    public void mouseClicked(MouseEvent e) {}

    @Override
    public void mouseReleased(MouseEvent e) {}

    @Override
    public void mouseEntered(MouseEvent e) {}

    @Override
    public void mouseExited(MouseEvent e) {}

    @Override
    public void valueChanged(TreeSelectionEvent e)
    {
        // в модели следует "выделить" текущего агента
        final TreeNode curT = (TreeNode) _mvcViewRef.getTree().getLastSelectedPathComponent();
        if (curT == null) return;
        if (curT.toString().startsWith("Agent"))
        {
            String agent = curT.toString().split(SPLIT)[1];
            String process = curT.getParent().toString().split(SPLIT)[1];
            String node = curT.getParent().getParent().toString().split(SPLIT)[1];
            _mvcModelRef.useAgent(agent, process, node);
            Agent ag = _mvcModelRef.getCurrentAgent();
            _mvcViewRef.refreshVariables(ag.getVariables());
            _mvcViewRef.refreshEvents(ag.getEvents());
            _mvcViewRef.refreshFids(ag.getFids());
        }
    }

    @Override
    public void valueChanged(ListSelectionEvent e)
    {
        _mvcViewRef.reloadFunctionText();
        _mvcViewRef.reloadVariableDefinition();
    }
}
