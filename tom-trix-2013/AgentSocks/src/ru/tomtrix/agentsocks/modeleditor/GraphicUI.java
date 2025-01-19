package ru.tomtrix.agentsocks.modeleditor;


import java.awt.*;
import java.util.*;
import java.io.File;
import javax.swing.*;
import java.awt.event.*;
import javax.swing.tree.*;
import org.apache.log4j.Logger;
import ru.tomtrix.agentsocks.mathmodel.Agent;
import ru.tomtrix.agentsocks.infrastructure.*;

import javax.swing.filechooser.FileNameExtensionFilter;

/**
 * few
 */
public class GraphicUI extends JFrame
{
    private final MVCModel _mvcModelRef;
    private final JTextField _statusBar = new JTextField();
    private final JMenuBar _menu = new JMenuBar();
    private final JTree _tree = new JTree(new DefaultMutableTreeNode("<Right-click here>"));
    private final JSplitPane _split1 = new JSplitPane(JSplitPane.VERTICAL_SPLIT);
    private final JSplitPane _split2 = new JSplitPane(JSplitPane.VERTICAL_SPLIT);
    private final JList<String> _variables = new JList<>(new String[] {"<No variables>"});
    private final JList<String> _events = new JList<>(new String[] {"<No events>"});
    private final JList<String> _fids = new JList<>(new String[] {"<No fids>"});
    private final JTextArea _functionBox = new JTextArea();
    private final JList<String> _rules = new JList<>(new String[] {"<No rules>"});

    public GraphicUI(MVCModel model)
    {
        super("Model Editor");
        if (model==null) throw new NullPointerException("gdr");
        _mvcModelRef = model;
    }

    private static void expandAll(JTree tree, TreePath parent, boolean expand)
    {
        TreeNode node = (TreeNode) parent.getLastPathComponent();
        if (node.getChildCount() >= 0)
            for (Enumeration e=node.children(); e.hasMoreElements();)
                expandAll(tree, parent.pathByAddingChild(e.nextElement()), expand);
        if (expand)
            tree.expandPath(parent);
        else
            tree.collapsePath(parent);
    }

    private static void selectNode(JTree tree, TreePath parent, String node)
    {
        TreeNode n = (TreeNode) parent.getLastPathComponent();
        if (n.toString().equals(node))
            tree.setSelectionPath(parent);
        else if (n.getChildCount() >= 0)
            for (Enumeration e=n.children(); e.hasMoreElements();)
                selectNode(tree, parent.pathByAddingChild(e.nextElement()), node);
    }

    void rebuildTreeByModel()
    {
        try
        {
            Model model = _mvcModelRef.get_model();
            if (model==null) return;
            String _lastTreeNode = _tree.getLastSelectedPathComponent().toString();
            DefaultTreeModel tm = (DefaultTreeModel)_tree.getModel();
            DefaultMutableTreeNode root = (DefaultMutableTreeNode) tm.getRoot();
            root.removeAllChildren();
            root.setUserObject(new DefaultMutableTreeNode("Model " +model.get_name()));
            for (Node node : model.get_nodes())
            {
                DefaultMutableTreeNode childNode = new DefaultMutableTreeNode("Node " + node.get_rank());
                root.add(childNode);
                for (LogicProcess p : node.get_container().get_processes())
                {
                    DefaultMutableTreeNode childProcess = new DefaultMutableTreeNode("Process " + p.get_name());
                    childNode.add(childProcess);
                    for (Agent agent : p.get_agents())
                        childProcess.add(new DefaultMutableTreeNode("Agent " + agent.get_name()));
                }
            }
            tm.reload();            //на эту херотень я потратил 3,5 часа!!!
            expandAll(_tree, new TreePath(root), true);
            selectNode(_tree, new TreePath(root), _lastTreeNode);
            if (_tree.getSelectionCount()==0) _tree.setSelectionRow(0);
        }
        catch (IllegalAccessException e)
        {
            Logger.getLogger(getClass()).error("frs", e);
        }
    }

    public void run()
    {
        GUIController controller = new GUIController(_mvcModelRef, this);

        // tree
        rebuildTreeByModel();
        _tree.setName("tree");
        _tree.setSelectionRow(0);
        _tree.addMouseListener(controller);
        _tree.addTreeSelectionListener(controller);
        _tree.setPreferredSize(new Dimension(200, 0));
        _tree.setBorder(BorderFactory.createTitledBorder("Model structure"));

        // function textbox
        _functionBox.setBorder(BorderFactory.createTitledBorder("Function definition"));

        // status bar
        _statusBar.setBackground(Color.lightGray);
        _statusBar.setDisabledTextColor(Color.black);
        _statusBar.setEnabled(false);

        // fids listbox
        _fids.setName("fids");
        _fids.setSelectedIndex(0);
        _fids.addMouseListener(controller);
        _fids.addListSelectionListener(controller);
        _fids.setBorder(BorderFactory.createTitledBorder("Function identificators"));

        // variables listbox
        _variables.setSelectedIndex(0);
        _variables.setName("variables");
        _variables.addMouseListener(controller);
        _variables.addListSelectionListener(controller);
        _variables.setBorder(BorderFactory.createTitledBorder("Variables"));

        // events listbox
        _events.setName("events");
        _events.setSelectedIndex(0);
        _events.addMouseListener(controller);
        _events.setBorder(BorderFactory.createTitledBorder("Events"));

        // rules listbox
        _rules.setName("rules");
        _rules.setSelectedIndex(0);
        _rules.addMouseListener(controller);

        // menu
        JMenu file = new JMenu("File");
        JMenuItem load = new JMenuItem("Load model");
        load.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                JFileChooser fch = new JFileChooser(System.getProperty("user.dir"));
                fch.setFileFilter(new FileNameExtensionFilter("Model files (*xml, *txt)", "xml", "txt"));
                if (fch.showOpenDialog(null) != JFileChooser.APPROVE_OPTION) return;
                _mvcModelRef.loadModel(fch.getSelectedFile().getName());
                rebuildTreeByModel();
            }
        });
        JMenuItem save = new JMenuItem("Save model");
        save.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (_mvcModelRef.get_model() == null) return;
                JFileChooser fch = new JFileChooser(System.getProperty("user.dir"));
                fch.setFileFilter(new FileNameExtensionFilter("Model files (*xml, *txt)", "xml", "txt"));
                fch.setSelectedFile(new File(_mvcModelRef.get_model().get_name() + ".xml"));
                if (fch.showSaveDialog(null) != JFileChooser.APPROVE_OPTION) return;
                String filename = fch.getSelectedFile().getName().trim();
                if (!filename.toLowerCase().endsWith(".xml"))
                    filename += ".xml";
                _mvcModelRef.saveModel(filename);
            }
        });
        JMenuItem test = new JMenuItem("Test...");
        save.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (_mvcModelRef.get_model() == null) return;
                _mvcModelRef.get_model().get_environment().put("1", new String[] {"meat", "sausage", "cheese"});
                _mvcModelRef.get_model().get_environment().put("2", new String[] {"tomato", "potato", "zrazy"});
                _mvcModelRef.get_model().get_environment().put("3", new String[] {"milk", "sourcream", "carrot"});
            }
        });
        JMenuItem exit = new JMenuItem("Exit");
        exit.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                System.exit(0);
            }
        });
        file.add(load);
        file.add(save);
        file.add(test);
        file.addSeparator();
        file.add(exit);
        _menu.add(file);

        // splitContainer 1
        _split1.setBottomComponent(new JScrollPane(_functionBox));
        _split1.setTopComponent(new JScrollPane(_fids));
        _split1.setDividerLocation(100);

        // splitContainer 2
        _split2.setBottomComponent(new JScrollPane(_events));
        _split2.setTopComponent(new JScrollPane(_variables));
        _split2.setDividerLocation(120);
        JPanel panel = new JPanel(new BorderLayout());
        panel.setPreferredSize(new Dimension(200, 0));
        panel.add(_split2);

        // form
        setSize(900, 400);
        setMinimumSize(new Dimension(600, 300));
        setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        try { UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName()); }
        catch (Exception ignore) {}
        setJMenuBar(_menu);
        getContentPane().add(panel, BorderLayout.EAST);
        getContentPane().add(_split1, BorderLayout.CENTER);
        getContentPane().add(_statusBar, BorderLayout.SOUTH);
        getContentPane().add(new JScrollPane(_tree), BorderLayout.WEST);
        setVisible(true);
    }

    public void setStatus(String s)
    {
        _statusBar.setText(s);
    }

    public void refreshVariables(Collection<String> vars)
    {
        _variables.removeAll();
        _variables.setListData(vars==null || vars.isEmpty() ? new String[] {"<No variables>"} : vars.toArray(new String[vars.size()]));
        if (_variables.isSelectionEmpty())
            _variables.setSelectedIndex(0);
    }

    public void refreshEvents(Collection<String> events)
    {
        _events.removeAll();
        _events.setListData(events==null || events.isEmpty() ? new String[] {"<No events>"} : events.toArray(new String[events.size()]));
        if (_events.isSelectionEmpty())
            _events.setSelectedIndex(0);
    }

    public void refreshFids(Collection<String> fids)
    {
        _fids.removeAll();
        _fids.setListData(fids==null || fids.isEmpty() ? new String[] {"<No fids>"} : fids.toArray(new String[fids.size()]));
        if (_fids.isSelectionEmpty())
            _fids.setSelectedIndex(0);
    }

    public void refreshRules(Collection<String> rules)
    {
        _rules.removeAll();
        _rules.setListData(rules==null || rules.isEmpty() ? new String[] {"<No rules>"} : rules.toArray(new String[rules.size()]));
        if (_rules.isSelectionEmpty())
            _rules.setSelectedIndex(0);
    }

    public void reloadFunctionText()
    {
        String fid = _fids.getSelectedValue();
        if (fid==null) return;
        _functionBox.setText(fid.startsWith("<") ? "" : _mvcModelRef.getFunctionByFid(fid));
    }

    public void reloadVariableDefinition()
    {
        String var = _variables.getSelectedValue();
        if (var==null) return;
        _statusBar.setText(var.startsWith("<") ? "" : _mvcModelRef.getVariableDefinition(var));
    }

    public JTree getTree()
    {
        return _tree;
    }

    public JList getVariablesListbox()
    {
        return _variables;
    }

    public JList<String> getEventsListbox()
    {
        return _events;
    }

    public JList<String> getFidsListbox()
    {
        return _fids;
    }

    public JList<String> getRulesTextbox()
    {
        return _rules;
    }

    public String getFunctionText()
    {
        return _functionBox.getText();
    }
}
