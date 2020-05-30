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
import { VueNotification } from '@/types/AwnTypes';
import dateTimeFilter from "@/utilities/filters/DateTime";
import VueDraggableResizable from 'vue-draggable-resizable'
import VueAWN from "vue-awesome-notifications"

import Nprogress from "nprogress"

import eventBus from "@/utilities/EventBus";

import Vuebar from 'vuebar';

import 'vue-draggable-resizable/dist/VueDraggableResizable.css'
import { VLazyImagePlugin } from "@/utilities/LazyImage";


dateTimeFilter(Vue);

Nprogress.configure({ showSpinner: false });

Vue.use(VLazyImagePlugin);

Vue.component('vue-draggable-resizable', VueDraggableResizable)

Vue.use(Vuebar);

// Your custom options
let options = {}

Vue.use(VueAWN, options)

library.add({faSpinner, faCheck, faTimes, faAngleDoubleUp, faAngleDoubleDown, faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines})

Vue.component('font-awesome-icon', FontAwesomeIcon)

Vue.use(checkView)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')