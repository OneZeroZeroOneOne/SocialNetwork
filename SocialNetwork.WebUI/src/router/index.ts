import Vue from 'vue'
import VueRouter from 'vue-router'
import PostView from '../views/PostView.vue'
import MainView from '../views/MainView.vue'
import BoardView from '../views/BoardView.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/notfound',
    name: 'notfound',
    component: () => import('../views/NotFoundView.vue')
  },
  {
    path: '/:boardname',
    name: 'board',
    component: BoardView
  },
  {
    path: '/',
    name: '',
    component: MainView
  },
  {
    path: '/post',
    name: 'post',
    component: PostView
  },
  {
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '*',
    name: 'notfound',
    component: () => import('../views/NotFoundView.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
