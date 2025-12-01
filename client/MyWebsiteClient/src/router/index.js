import { createRouter, createWebHistory } from "vue-router";
import store from "@/store";
import Home from "@/views/Home.vue";
import About from "@/views/About.vue";
import Contact from "@/views/Contact.vue";
import Project from "@/views/Project.vue";
import Article from "@/views/Article.vue";
import ArticleDetail from "@/components/Article/ArticleDetail.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: Home,
    },
    {
      path: "/about",
      name: "about",
      component: About,
    },
    {
      path: "/contact",
      name: "contact",
      component: Contact,
    },
    {
      path: "/project",
      name: "project",
      component: Project,
    },
    {
      path: "/articles",
      name: "article",
      component: Article,
    },
    {
      path: "/articles/:slug",
      name: "Article-Detail",
      component: ArticleDetail,
    },
    {
      path: "/login",
      name: "Login",
      component: () => import("@/views/Login.vue"),
      meta: { guest: true },
    },
    {
      path: "/register",
      name: "Register",
      component: () => import("@/views/Register.vue"),
      meta: { guest: true },
    },
    {
      path: "/admin",
      name: "Admin",
      component: () => import("@/views/AdminPage.vue"),
      meta: {
        requiresAuth: true,
        requiresRole: "Admin",
        hideNavigation: true,
      },
    },
    {
      path: "/admin/categories",
      name: "AdminCategories",
      component: () => import("@/views/Admin/Category/CategoryManagement.vue"),
      meta: {
        requiresAuth: true,
        requiresRole: "Admin",
        hideNavigation: true,
      },
    },
    {
      path: "/admin/projects",
      name: "AdminProjects",
      component: () => import("@/views/Admin/Project/ProjectManagement.vue"),
      meta: {
        requiresAuth: true,
        requiresRole: "Admin",
        hideNavigation: true,
      },
    },
    {
      path: "/admin/users",
      name: "AdminUsers",
      component: () => import("@/views/Admin/User/UserManagement.vue"), // Bu dosyayı oluşturmanız gerekecek
      meta: {
        requiresAuth: true,
        requiresRole: "Admin",
        hideNavigation: true,
      },
    },
    {
      path: "/admin/articles",
      name: "AdminArticles",
      component: () => import("@/views/Admin/Article/ArticleManagement.vue"), // Bu dosyayı oluşturmanız gerekecek
      meta: {
        requiresAuth: true,
        requiresRole: "Admin",
        hideNavigation: true,
      },
    },
    {
      path: "/admin/settings",
      name: "AdminSettings",
      component: () => import("@/views/Admin/System/SystemSettings.vue"), // Bu dosyayı oluşturmanız gerekecek
      meta: {
        requiresAuth: true,
        requiresRole: "Admin",
        hideNavigation: true,
      },
    },
    // Tüm tanımlı olmayan rotaları yakalamak için (404 sayfası)
    {
      path: "/:pathMatch(.*)*",
      name: "NotFound",
      component: () => import("@/views/NotFound.vue"),
    },
  ],
});

// Navigation Guard
router.beforeEach((to, from, next) => {
  const isAuthenticated = store.getters["auth/isAuthenticated"];
  const currentUser = store.getters["auth/currentUser"];
  const isTokenExpired = store.getters["auth/isTokenExpired"];

  // Token süresi dolmuşsa logout yap
  if (isAuthenticated && isTokenExpired) {
    store.dispatch("auth/logout");
    next("/login");
    return;
  }

  // 1. Giriş gerektiren sayfalar (requiresAuth)
  if (to.meta.requiresAuth && !isAuthenticated) {
    next("/login");
    return;
  }

  // 2. Rol Gerektiren Sayfalar (requiresRole)
  if (to.meta.requiresRole && isAuthenticated) {
    const requiredRole = to.meta.requiresRole;
    if (!currentUser || currentUser.role !== requiredRole) {
      // Notification store varsa kullan
      if (store.hasModule('notification')) {
        store.dispatch(
          "notification/showNotification",
          {
            message: "Bu sayfaya erişim yetkiniz bulunmamaktadır.",
            type: "error",
          },
          { root: true }
        );
      } else {
        console.warn("Bu sayfaya erişim yetkiniz bulunmamaktadır.");
      }
      next("/");
      return;
    }
  }

  // 3. Giriş yapmış kullanıcı login/register sayfasına gidemez (guest)
  if (to.meta.guest && isAuthenticated) {
    next("/");
    return;
  }

  next();
});

export default router;