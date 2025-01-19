package ru.tomtrix.agentsocks;

import java.util.*;
import java.io.File;
import org.apache.log4j.Logger;
import org.semanticweb.owlapi.model.*;
import ru.tomtrix.productions.Variable;
import ru.tomtrix.productions.core.Core;
import org.semanticweb.owlapi.apibinding.OWLManager;


/**
 * fesgrht
 */
public class AgentProductionCore extends Core
{
    private final Object _runtimeAssistantRef;
    private final OWLOntology _ontology;

    /**
     * fadefse
     */
    public AgentProductionCore(Object runtimeAssistant) throws OWLOntologyCreationException
    {
        super();
        _runtimeAssistantRef = runtimeAssistant;
        //TODO (онтология д.б. заменяемой)
        _ontology = OWLManager.createOWLOntologyManager().loadOntologyFromOntologyDocument(new File("protege.owl"));
    }

    private List<String> getSubclasses(String concept)
    {
        List<String> result = new ArrayList<>();
        for (OWLClass clazz : _ontology.getClassesInSignature())
        {
            if (!concept.toLowerCase().equals(clazz.toString().toLowerCase().split("[#>]")[1])) continue;
            for (OWLClassExpression sub : clazz.getSubClasses(_ontology))
                result.add(sub.toString().toLowerCase().split("[#>]")[1]);
        }
        return result;
    }

    @Override
    public String askForVariable(Variable variable)
    {
        try
        {
            // пытаемся получить значение из поля Runtime Assistant (через Java Reflection)
            try { return _runtimeAssistantRef.getClass().getField(variable.toString()).get(_runtimeAssistantRef).toString();}
            catch (Exception ignored) {}
            // what a fuck? Такого поля нет! Попробуем поискать сведения о субклассах концепта в онтологии
            // TODO (очевидно, здесь пока задачезависимая реализация)
            List<String> s1 = getSubclasses(variable.toString());
            ArrayList s2 = (ArrayList)_runtimeAssistantRef.getClass().getField("cart").get(_runtimeAssistantRef);
            s1.retainAll(s2);
            if (s1.size() > 0) return "yes";
            return "no";
        }
        catch (Exception e) { Logger.getLogger(getClass()).error(String.format("Cannot extract the value from the Runtime Assistant [%s]", variable), e); }
        return "undefined";
    }
}
