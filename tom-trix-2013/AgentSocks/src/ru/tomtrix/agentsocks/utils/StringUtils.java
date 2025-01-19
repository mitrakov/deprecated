package ru.tomtrix.agentsocks.utils;

import java.util.Collection;

/**
 * fse
 */
public class StringUtils
{
    public static String getElements(Collection<?> list, CharSequence separator)
    {
        StringBuilder sb = new StringBuilder();
        for (Object ob : list)
            sb.append(separator).append(ob);
        return sb.substring(separator.length());
    }
}
