import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import '@/styles/nprogress.css';
import '@/styles/scss/animation.scss';
import checkView from 'vue-check-view'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faSpinner, faCheck, faTimes, faAngleDoubleUp, faAngleDoubleDown, faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import setup from './utilities/math_round_extensions.js'
import { VueNotification } from '@/types/AwnTypes';
import VueDraggableResizable from 'vue-draggable-resizable'
import VueAWN from "vue-awesome-notifications"

import Nprogress from "nprogress"

Nprogress.configure({ showSpinner: false });

import { VLazyImagePlugin } from "@/utilities/LazyImage";
Vue.use(VLazyImagePlugin);

// optionally import default styles
import 'vue-draggable-resizable/dist/VueDraggableResizable.css'

Vue.component('vue-draggable-resizable', VueDraggableResizable)
// require styles
import moment from 'moment'
import VueLazyload from 'vue-lazyload'
import eventBus from "@/utilities/EventBus";


Vue.use(VueLazyload)

import Vuebar from 'vuebar';
Vue.use(Vuebar);

// or with options
Vue.use(VueLazyload, {
  preLoad: 2.5,
  error: 'dist/error.png',
  loading: 'dist/loading.gif',
  attempt: 1,
  silent: false,
})

Vue.filter('formatDate', function(value) {
  if (value) {
    moment.locale(navigator.language)
    return moment(String(value)).format('LLLL')
  }
})
// Your custom options
let options = {}

Vue.use(VueAWN, options)

setup()

library.add({faSpinner, faCheck, faTimes, faAngleDoubleUp, faAngleDoubleDown, faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines})

Vue.component('font-awesome-icon', FontAwesomeIcon)

Vue.use(checkView)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')