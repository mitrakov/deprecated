package com.mitrakoff.self.view.widgets;

import com.codename1.components.InteractionDialog;
import com.codename1.ui.Button;
import com.codename1.ui.Form;
import com.codename1.ui.TextField;
import com.codename1.ui.layouts.BoxLayout;
import java.util.*;

public class TypeaheadText extends TextField {
    private final InteractionDialog popup = new InteractionDialog(BoxLayout.y());
    public TypeaheadText(String text, String hint, Iterable<String> suggestions) {
        super(text, hint);
        //popup.setDisposeWhenPointerOutOfBounds(true);
        addDataChangedListener((t, i) -> {
            final String txt = getText().trim().toLowerCase();

            if (!txt.isEmpty()) {
                final List<String> filteredList = new ArrayList<>(); // Stream API is not supported
                for (String s : suggestions) {
                    if (filteredList.size() > 12) break;
                    if (s.toLowerCase().contains(txt))
                        filteredList.add(s);
                }

                if (!filteredList.isEmpty()) {
                    popup.removeAll();
                    for (String s : filteredList) {
                        final Button suggestionButton = new Button(s);
                        suggestionButton.addActionListener(e -> {
                            setText(s);
                            popup.setHidden(true);
                        });
                        popup.add(suggestionButton);
                    }

                    popup.showPopupDialog(this);
                } else popup.setHidden(true); // if no suggestions, hide the popup
            } else popup.setHidden(true);     // if text field is empty, hide the popup

            // getComponentForm().revalidate();
        });
    }
}
