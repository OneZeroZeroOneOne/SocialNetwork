import Vue from 'vue'
import VueRouter from 'vue-router'
import PostView from '../views/PostView.vue'
import MainView from '../views/MainView.vue'
import BoardView from '../views/BoardView.vue'
import GalleryView from '../views/GalleryView.vue'
import InfoView from '../views/InfoView.vue'
import ContactsView from '../views/ContactsView.vue'

import Nprogress from "nprogress"
import EventBus from '@/utilities/EventBus'

Vue.use(VueRouter)

const routes = [
  {
    path: '/notfound',
    name: 'notfound',
    component: () => import('../views/NotFoundView.vue'),  
    props: true
  },
  {
    path: '/poll',
    name: 'poll',
    component: () => import('../components/Contents/PollComponent.vue'),
  },
  {
    path: '/contacts',
    name: 'contacts',
    component: ContactsView
  },
  {
    path: '/info',
    name: 'info',
    component: InfoView
  },
  {
    path: '/gallery',
    name: 'gallery',
    component: GalleryView
  },
  {
    path: '/:boardname',
    name: 'board',
    component: BoardView
  },
  {
    path: '/:boardname/:postid',
    name: 'post',
    component: PostView
  },
  {
    path: '/',
    name: '',
    component: MainView
  },
  {
    path: '*',
    name: 'notfound_wildcard',
    component: () => import('../views/NotFoundView.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  Nprogress.start()
  EventBus.emit("clear-attachments");
  next()
})

export default router
