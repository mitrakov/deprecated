package ru.tomtrix.productions.core;

import java.io.*;
import ru.tomtrix.productions.Variable;

/**
 * Класс расширяет ядро оболочки экспертной системы. Запрашиваемые переменные получаем из консоли
 */
public class ConsoleCore extends Core
{
    private BufferedReader _reader = new BufferedReader(new InputStreamReader(System.in));

    public ConsoleCore(Variable ... variables)
    {
        super(variables);
    }

    @Override
    public String askForVariable(Variable variable)
    {
        try
        {
            System.out.println(variable + "?");
            return _reader.readLine();
        }
        catch (IOException e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
