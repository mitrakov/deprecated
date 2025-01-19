package ru.tomtrix.agentsocks.utils;

import java.io.*;
import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.StaxDriver;

/**
 *rwr3w
 */
@SuppressWarnings("unused, unchecked")
public class XMLSerializer<T>
{
    private XStream _xstream = new XStream(new StaxDriver());

    public String serializeToString(T obj)
    {
        return _xstream.toXML(obj);
    }

    public void serializeToFile(T obj, String filename) throws IOException
    {
        Writer writer = new FileWriter(filename);
        _xstream.toXML(obj, writer);
        writer.close();
    }

    public T deserializeFromString(String s)
    {
        return (T)_xstream.fromXML(s);
    }

    public T deserializeFromFile(String filename) throws IOException
    {
        Reader reader = new FileReader(filename);
        T result = (T)_xstream.fromXML(reader);
        reader.close();
        return result;
    }
}
