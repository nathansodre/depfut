import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import { VDataTable } from "vuetify/labs/VDataTable";

export default defineNuxtPlugin((nuxtApp) => {
  const vuetify = createVuetify({
    components: {
      VDataTable,
      ...components,
    },
    directives,
    theme: {
      defaultTheme: "dark",
    },
  });

  nuxtApp.vueApp.use(vuetify);
});
