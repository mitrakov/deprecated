package ru.tomtrix.mpiagent.tester;

import mpi.MPI;
import javax.swing.*;
import java.awt.event.*;
import ru.tomtrix.mpiagent.*;
import javax.swing.border.EmptyBorder;

/** Tester class that contains a user interface and an interface method <i>messageReceived()</i>
 * @author tom-trix */
public class Tester extends JFrame implements MPIAgentListener
{
	private static final long	serialVersionUID	= 2348369553744039293L;
	private JPanel				contentPane;
	private JTextField			textField;

	/** Create the frame. */
	public Tester()
	{
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 285, 96);
		try
		{
			setTitle("Node " + MPI.COMM_WORLD.Rank());
		}
		catch (Exception e)
		{
			setTitle("Node");
		}
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(null);

		textField = new JTextField();
		textField.setBounds(12, 35, 114, 19);
		contentPane.add(textField);
		textField.setColumns(10);

		JButton btnSend = new JButton("Send");
		btnSend.addActionListener(new ActionListener()
		{
			public void actionPerformed(ActionEvent arg0)
			{
				try
				{
					String strs[] = textField.getText().trim().split(" ");
					byte data[] = new byte[strs.length];
					for (int i = 0; i < strs.length; i++)
						data[i] = Byte.parseByte(strs[i]);
					MPIAgent.getInstance().send(data, MPI.COMM_WORLD.Rank() == 0 ? 1 : 0);
				}
				catch (Exception e)
				{
					JOptionPane.showMessageDialog(contentPane, e);
					e.printStackTrace();
				}
			}
		});
		btnSend.setBounds(138, 32, 117, 25);
		contentPane.add(btnSend);
		
		JLabel lblNewLabel = new JLabel("Input numbers separated by space");
		lblNewLabel.setBounds(12, 8, 257, 15);
		contentPane.add(lblNewLabel);
		setVisible(true);
	}

	@Override
	public void messageReceived(byte[] data, int source, int tag)
	{
		StringBuffer sb = new StringBuffer(String.format("Received from %d: ", source));
		for (byte i : data)
			sb.append(i).append(", ");
		JOptionPane.showMessageDialog(contentPane, sb.substring(0, sb.length() - 2));
	}
}
