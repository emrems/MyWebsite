<template>
  <div class="article-hero">
    <div
      class="article-hero-image"
      :style="{
        backgroundImage: `url(${heroImage})`,
      }"
    ></div>
    <div class="article-hero-content">
      <div class="article-breadcrumb">
        <RouterLink to="/">Ana Sayfa</RouterLink>
        <span class="breadcrumb-separator">/</span>
        <RouterLink to="/articles">Blog</RouterLink>
        <span class="breadcrumb-separator">/</span>
        <span>{{ article.title }}</span>
      </div>

      <h1 class="article-title">{{ article.title }}</h1>

      <div class="article-meta">
        <div class="meta-item">
          <i class="icon-calendar"></i>
          <span>{{ formattedDate }}</span>
        </div>
        <div class="meta-item">
          <i class="icon-clock"></i>
          <span>{{ readingTime }} dk okuma</span>
        </div>
        <div class="meta-item">
          <i class="icon-user"></i>
          <span>Yazar: {{ article.authorName }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ArticleHero',
  
  props: {
    article: {
      type: Object,
      required: true
    },
    readingTime: {
      type: Number,
      required: true
    }
  },

  computed: {
    formattedDate() {
      const date = new Date(this.article.createdDate);
      return date.toLocaleDateString("tr-TR", {
        year: "numeric",
        month: "long",
        day: "numeric",
      });
    },

    heroImage() {
      // MediaFiles'dan ilk Image tipindeki resmi al (hero için)
      if (this.article.mediaFiles && this.article.mediaFiles.length > 0) {
        const firstImage = this.article.mediaFiles.find(m => m.type === 0); // 0 = Image
        if (firstImage) {
          return firstImage.url;
        }
      }
      // Fallback: eğer resim yoksa placeholder
      return `https://picsum.photos/1200/600?random=${this.article.id}`;
    }
  }
};
</script>

<style scoped>
.article-hero {
  position: relative;
  margin-bottom: 2rem;
}

.article-hero-image {
  height: 400px;
  background-size: cover;
  background-position: center;
  position: relative;
}

.article-hero-image:after {
  content: "";
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 50%;
  background: linear-gradient(to bottom, transparent 0%, rgba(0, 0, 0, 0.7) 100%);
}

.article-hero-content {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  padding: 2rem;
  color: white;
  z-index: 2;
}

.article-breadcrumb {
  margin-bottom: 1rem;
  font-size: 0.9rem;
}

.article-breadcrumb a {
  color: rgba(255, 255, 255, 0.8);
  text-decoration: none;
}

.article-breadcrumb a:hover {
  color: white;
  text-decoration: underline;
}

.breadcrumb-separator {
  margin: 0 0.5rem;
  color: rgba(255, 255, 255, 0.6);
}

.article-title {
  font-size: 2.8rem;
  font-weight: 700;
  line-height: 1.2;
  margin-bottom: 1.5rem;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
}

.article-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  margin-bottom: 1rem;
}

.meta-item {
  display: flex;
  align-items: center;
  font-size: 0.9rem;
  color: rgba(255, 255, 255, 0.9);
}

.meta-item i {
  margin-right: 0.5rem;
  font-size: 1rem;
}

/* İkonlar */
.icon-calendar:before {
  content: "📅";
}
.icon-clock:before {
  content: "🕒";
}
.icon-user:before {
  content: "👤";
}

@media (max-width: 768px) {
  .article-hero-image {
    height: 300px;
  }

  .article-title {
    font-size: 2rem;
  }

  .article-meta {
    flex-direction: column;
    gap: 0.8rem;
  }
}
</style>