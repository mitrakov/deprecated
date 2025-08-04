package com.mitrakoff.self.view;

import com.codename1.components.InteractionDialog;
import com.codename1.components.ToastBar;
import com.codename1.ui.*;
import com.codename1.ui.layouts.BorderLayout;
import com.codename1.ui.layouts.BoxLayout;
import com.mitrakoff.self.model.Model;
import static com.codename1.ui.CN.*;
import static com.mitrakoff.self.view.AppResources.*;

public class MainScreen extends Form {
    private final Model model;
    private final Button btnMain;

    public MainScreen(String title, Model model, Form adminScreen) {
        super(title, BorderLayout.center());
        this.model = model;

                     btnMain = new Button(model.getCurrentKey(), "TommyBlack5mm");
        final Button btnHelp = new Button(FontImage.createMaterial(FontImage.MATERIAL_HELP, "TommyGreen", 10));
        final Button btnEdit = new Button(FontImage.createMaterial(FontImage.MATERIAL_INSERT_COMMENT, "TommyBlue", 10));
        final Button btnFlag = new Button(model.getCurrentLanguage().equals(Model.enGB) ? flagEnGb : flagEsEs);
        final InteractionDialog switchLangMenu = buildSwitchLanguageMenu(btnFlag);

        btnMain.addActionListener(e -> onButtonMainClick());
        btnHelp.addActionListener(e -> Dialog.show(model.getCurrentKey(), model.getCurrentTranslation(), "OK", null));
        btnEdit.addActionListener(e -> onButtonEditClick(adminScreen));
        btnFlag.addActionListener(e -> switchLangMenu.showPopupDialog(btnFlag));

        add(CENTER, btnMain);
        add(NORTH, new Container(BoxLayout.xRight()).add(btnFlag));
        add(SOUTH, new Container(BoxLayout.xRight()).add(new Container(BoxLayout.y()).addAll(btnHelp, btnEdit)));
    }

    private void onButtonMainClick() {
        ToastBar.showMessage(model.getCurrentTranslation(), FontImage.MATERIAL_INFO, 1500);
        updateMainButton();
    }

    private void updateMainButton() {
        btnMain.setText(model.next());
        btnMain.getParent().revalidate();
    }

    private void onButtonEditClick(Form form) {
        form.show();
    }

    private InteractionDialog buildSwitchLanguageMenu(Button target) {
        final InteractionDialog menu = new InteractionDialog(BoxLayout.y());
        final Button btnEnGb = new Button(AppResources.flagEnGb);
        final Button btnEsEs = new Button(AppResources.flagEsEs);

        btnEnGb.addActionListener(e -> {
            menu.dispose();
            model.setLanguage(Model.enGB);
            target.setIcon(AppResources.flagEnGb);
            updateMainButton();
        });

        btnEsEs.addActionListener(e -> {
            menu.dispose();
            model.setLanguage(Model.esES);
            target.setIcon(AppResources.flagEsEs);
            updateMainButton();
        });

        menu.addAll(btnEnGb, btnEsEs);
        menu.setDisposeWhenPointerOutOfBounds(true);
        return menu;
    }
}
