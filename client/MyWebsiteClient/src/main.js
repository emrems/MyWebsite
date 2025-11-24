import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import '@fortawesome/fontawesome-free/css/all.min.css'

import NotificationPlugin from './plugins/notifications';

const app = createApp(App)

app.use(router)

app.use(NotificationPlugin);
app.use(store)
app.mount('#app')
