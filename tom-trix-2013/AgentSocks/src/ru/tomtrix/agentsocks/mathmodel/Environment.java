package ru.tomtrix.agentsocks.mathmodel;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import ru.tomtrix.agentsocks.infrastructure.Model;

/**
 * fse
 */
public class Environment
{
    private static Model _modelStaticRef;

    public static Object get(String variable)
    {
        return _modelStaticRef.get_environment()._env.get(variable.trim());
    }

    private final Model _modelRef;
    private final Map<String, Object> _env = new ConcurrentHashMap<>();

    public Environment(Model model)
    {
        _modelStaticRef = model;
        _modelRef = model;
    }

    /** Выполняет адское извращенство: копирует модель из обычной ссылки в статическую ссылку,
     * чтобы затем можно было обратиться к текущему экземпляру Environment методом get(String) */
    public void init()
    {
        _modelStaticRef = _modelRef;
    }

    public void put(String variable, Object x)
    {
        if (variable == null || x == null || variable.trim().isEmpty()) throw new NullPointerException("dgnjut");
        _env.put(variable.trim(), x);
    }

    public void put(List<String> variables, List<Object> objects)
    {
        if (variables == null || objects == null || variables.isEmpty() || objects.isEmpty()) throw new NullPointerException("dgnjut");
        for (int i=0; i<Math.min(variables.size(), objects.size()); i++)
            if (variables.get(i) != null && objects.get(i) != null)
                _env.put(variables.get(i).trim(), objects.get(i));
    }
}
