import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'nprogress/nprogress.css';
import Notifications from 'vue-notification'
import checkView from 'vue-check-view'
import Plugin from './components/vue-js-modal/src/index.js'

Vue.use(Plugin, { dynamic: true, injectModalsContainer: true})

Vue.use(checkView)

Vue.use(Notifications)
Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
