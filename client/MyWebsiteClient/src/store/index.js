// store/index.js
import { createStore } from 'vuex';
import projects from './modules/projects';
import articles from './modules/articles';
import experience from './modules/experience';
import comments from './modules/comments';
import auth from './modules/auth'; // ⭐ Auth modülünü ekle
import categories from './modules/categories';

export default createStore({
  modules: {
    projects, 
    articles,
    experience,
    comments,
    auth, 
    categories
  }
});