<template>
  <header class="navbar">
    <div class="wrapper">
      <nav class="nav-links">
        <RouterLink to="/" class="nav-item">Ana Sayfa</RouterLink>
        <RouterLink to="/about" class="nav-item">Hakkımda</RouterLink>
        <RouterLink to="/contact" class="nav-item">İletişim</RouterLink>
        <RouterLink to="/articles" class="nav-item">Blog</RouterLink>
        <RouterLink to="/project" class="nav-item">Projeler</RouterLink>
      </nav>
    </div>
  </header>

  <RouterView />
   <Notification />
</template>

<style scoped>
/* Yeni stiller */
.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  background-color: var(--color-background-soft);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 1rem 2rem;
  z-index: 1000;
  display: flex;
  justify-content: center;
  align-items: center;
}

.wrapper {
  display: flex;
  justify-content: center;
  width: 100%;
}

.nav-links {
  display: flex;
  gap: 1.5rem;
}

.nav-item {
  color: var(--color-text);
  text-decoration: none;
  font-size: 1rem;
  padding: 0.5rem 1rem;
  transition: color 0.3s ease;
}

.nav-item:hover {
  color: var(--color-primary);
}

/* Mevcut stillerle çakışmaması için orijinal stilleri temizledim */
header,
nav,
nav a {
  /* Orijinal stillerin üzerine yazılması için boşaltıldı */
}

/* Orijinal medya sorgusunu güncelledim */
@media (min-width: 1024px) {
  .navbar {
    padding: 1rem 4rem;
  }
}
</style>