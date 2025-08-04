package com.mitrakoff.self.view;

import com.codename1.ui.*;
import com.codename1.ui.events.ActionEvent;
import com.codename1.ui.layouts.BoxLayout;
import com.codename1.ui.layouts.GridLayout;
import com.codename1.ui.plaf.RoundRectBorder;
import com.mitrakoff.self.model.Model;
import com.mitrakoff.self.view.widgets.TypeaheadText;

public class AdminScreen extends Form {
    private final Model model;

    public AdminScreen(String title, Model model) {
        super(title, BoxLayout.y());
        this.model = model;

        final TypeaheadText txtKey = new TypeaheadText("", "Key", model.getAllKeys());
        final TextField txtTranslation = new TextField("", "Translation");
        final Button btnUpsert = new Button("Upsert", FontImage.MATERIAL_INSERT_COMMENT, 5, "ab1");
        final Button btnDelete = new Button("Delete", FontImage.MATERIAL_DELETE, 5, "ab2");

        btnUpsert.setEnabled(false);
        btnUpsert.addActionListener(e -> onButtonUpsertClick(txtKey, txtTranslation));
        btnUpsert.getStyle().setBorder(RoundRectBorder.createLineBorder(7, 0x3322FF));

        btnDelete.setEnabled(false);
        btnDelete.addActionListener(e -> onButtonDeleteClick(txtKey));
        btnDelete.getStyle().setBorder(RoundRectBorder.createLineBorder(7, 0xFF2233));

        txtKey.addDataChangedListener((t, i) -> {
            final boolean keyEmpty = txtKey.getText().trim().isEmpty();
            btnUpsert.setEnabled(!keyEmpty);
            btnDelete.setEnabled(!keyEmpty);
        });

        add(txtKey).add(txtTranslation).add(GridLayout.encloseIn(2, btnUpsert, btnDelete));
    }

    public void setParentScreen(Form parent) {
        getToolbar().setBackCommand(new Command("") {
            @Override
            public void actionPerformed(ActionEvent evt) {
                parent.showBack();
            }
        });
    }

    private void onButtonUpsertClick(TextField txtKey, TextField txtTranslation) {
        final String key = txtKey.getText().trim();
        final String translation = txtTranslation.getText().trim();
        if (!key.isEmpty() && !translation.isEmpty())
            model.upsert(key, translation);
    }

    private void onButtonDeleteClick(TextField txt) {
        final String key = txt.getText().trim();
        if (!key.isEmpty())
            model.delete(key);
    }
}
