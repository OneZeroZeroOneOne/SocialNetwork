import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'nprogress/nprogress.css';
import checkView from 'vue-check-view'
import Plugin from './components/vue-js-modal/src/index.js'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import setup from './utilities/math_round_extensions.js'
import VueQuillEditor from 'vue-quill-editor'
import { VueNotification } from '@/types/AwnTypes';
import VueDraggableResizable from 'vue-draggable-resizable'
import VueAWN from "vue-awesome-notifications"

// optionally import default styles
import 'vue-draggable-resizable/dist/VueDraggableResizable.css'

Vue.component('vue-draggable-resizable', VueDraggableResizable)
// require styles
import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
import 'quill/dist/quill.bubble.css'

// Your custom options
let options = {}

Vue.use(VueAWN, options)
Vue.use(VueQuillEditor, /* { default global options } */)

setup()

library.add({faUndo, faRedo, faParagraph, faListOl, faListUl, faCode, faUnderline, faStrikethrough, faItalic, faBold, faQuoteLeft, faGripLines})

Vue.component('font-awesome-icon', FontAwesomeIcon)

Vue.use(Plugin, { dynamic: true, injectModalsContainer: true})

Vue.use(checkView)

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
