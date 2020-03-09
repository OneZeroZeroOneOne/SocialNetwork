import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'nprogress/nprogress.css';
import checkView from 'vue-check-view'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faAngleDoubleUp, faAngleDoubleDown, faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import setup from './utilities/math_round_extensions.js'
import { VueNotification } from '@/types/AwnTypes';
import VueDraggableResizable from 'vue-draggable-resizable'
import VueAWN from "vue-awesome-notifications"

// optionally import default styles
import 'vue-draggable-resizable/dist/VueDraggableResizable.css'

Vue.component('vue-draggable-resizable', VueDraggableResizable)
// require styles
import moment from 'moment'
import VueLazyload from 'vue-lazyload'
import LinkToComponent from '../src/components/MarkdownComponents/LinkToComponent.vue';
import GreenComponent from '../src/components/MarkdownComponents/GreenComponent.vue';
import SpoilerComponent from '../src/components/MarkdownComponents/SpoilerComponent.vue';

Vue.use(VueLazyload)

// or with options
Vue.use(VueLazyload, {
  preLoad: 1.5,
  error: 'dist/error.png',
  loading: 'dist/loading.gif',
  attempt: 1,
  silent: false,
})

Vue.filter('formatDate', function(value) {
  if (value) {
    return moment(String(value)).format('DD-MM-YYYY hh:mm:ss')
  }
})
// Your custom options
let options = {}

Vue.use(VueAWN, options)

setup()

library.add({faAngleDoubleUp, faAngleDoubleDown, faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines})

Vue.component('font-awesome-icon', FontAwesomeIcon)

Vue.use(checkView)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')

var getChildrenTextContent = function (children) {
  return children.map(function (node) {
    return node.children
      ? getChildrenTextContent(node.children)
      : node.text
  }).join('')
}

Vue.component('link-to', LinkToComponent)
Vue.component('green', GreenComponent)
Vue.component('sp', SpoilerComponent)