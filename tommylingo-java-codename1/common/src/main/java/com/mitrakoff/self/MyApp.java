package com.mitrakoff.self;

import com.codename1.system.Lifecycle;
import com.mitrakoff.self.model.Model;
import com.mitrakoff.self.view.AdminScreen;
import com.mitrakoff.self.view.MainScreen;

@SuppressWarnings("unused")
public class MyApp extends Lifecycle {
    @Override
    public void runApp() {
        final Model model = new Model();
        model.loadAll(v -> {
            final AdminScreen adminScreen = new AdminScreen("Tommylingo", model);
            final MainScreen  mainScreen  = new MainScreen ("Tommylingo", model, adminScreen);
            adminScreen.setParentScreen(mainScreen);
            mainScreen.show();
        });
    }
}
