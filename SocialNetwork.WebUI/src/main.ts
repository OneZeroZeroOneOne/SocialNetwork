import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'nprogress/nprogress.css';
import Notifications from 'vue-notification'
import checkView from 'vue-check-view'
import vmodal from 'vue-js-modal'

Vue.use(vmodal, { dynamic: true, injectModalsContainer: true})

Vue.use(checkView)

Vue.use(Notifications)
Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
