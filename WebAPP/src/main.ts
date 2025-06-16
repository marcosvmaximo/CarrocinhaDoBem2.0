import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

// Este é o método correto para iniciar uma aplicação baseada em NgModules.
// Ele inicializa o AppModule, que por sua vez inicializa o AppComponent.
platformBrowserDynamic().bootstrapModule(AppModule)
    .catch(err => console.error(err));