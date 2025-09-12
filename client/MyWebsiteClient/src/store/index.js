import { createStore } from 'vuex';
import projects from './modules/projects';
import articles from './modules/articles';


export default createStore({
  modules: {
    projects, // projects modülünü ekleyin
    articles
  }
});