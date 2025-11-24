<template>
  <div class="experience-list">

    <div v-if="isLoading" class="loading-message">
      Deneyimler yükleniyor...
    </div>

    <div v-if="error" class="error-message">
      Hata oluştu: {{ error }}
    </div>

    <div v-if="!isLoading && !error" class="accordion-wrapper">
      <div 
        v-for="(exp, index) in sortedExperiences" 
        :key="exp.id" 
        class="accordion-item"
        :class="{ 'is-active': activeIndex === index }"
      >
        
        <div class="accordion-header" @click="toggleAccordion(index)">
          <div class="header-info">
            <h3 class="company-title">{{ exp.company }}</h3>
            <span class="role-text">{{ exp.role }}</span>
          </div>
          <div class="header-meta">
            <span class="date-range">
              {{ format(exp.startDate) }} – {{ exp.endDate ? format(exp.endDate) : 'Devam Ediyor' }}
            </span>
            <span class="toggle-icon">{{ activeIndex === index ? '▲' : '▼' }}</span>
          </div>
        </div>

        <div class="accordion-content" v-show="activeIndex === index">
          <p class="description">{{ exp.description }}</p>
          </div>
      </div>
    </div>

  </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: "AllExperience",
  
  data() {
    return {
      activeIndex: null, // Açık olan akordiyonun index'i (hiçbiri açık değilse null)
    };
  },

  computed: {
    ...mapGetters("experience", ["allExperiences", "isLoading", "getError"]),

    experiences() {
      return this.allExperiences;
    },

    error() {
      return this.getError;
    },
    
    // Deneyimleri en yeniden eskiye sıralama (Tarih formatı varsayılarak)
    sortedExperiences() {
        return [...this.experiences].sort((a, b) => {
            const dateA = new Date(b.startDate);
            const dateB = new Date(a.startDate);
            return dateA - dateB;
        });
    }
  },

  methods: {
    format(date) {
      // Örn: '2023-10-01' tarihini 'Eki 2023' formatına dönüştürür.
      return new Date(date).toLocaleDateString('tr-TR', { year: 'numeric', month: 'short' });
    },
    
    toggleAccordion(index) {
      // Eğer tıklanan zaten açıksa, kapat (null yap). Değilse, aç.
      this.activeIndex = this.activeIndex === index ? null : index;
    }
  },

  created() {
    // Component açılınca API'den verileri çek
    this.$store.dispatch("experience/fetchExperiences");
  }
};
</script>

<style scoped>
.experience-list {
  padding: 0 1rem;
}

h2 {
  display: none; /* Başlık ana sayfada zaten var, burada gizle */
}

.loading-message, .error-message {
  text-align: center;
  padding: 20px;
  font-weight: 500;
}

.error-message {
  color: #dc3545;
}

/* Akordiyon Ana Yapı */
.accordion-wrapper {
  margin-top: 20px;
}

.accordion-item {
  border: 1px solid #e0e0e0;
  margin-bottom: 10px;
  border-radius: 8px;
  overflow: hidden;
  transition: all 0.3s ease;
  background-color: white;
}

.accordion-item.is-active {
  box-shadow: 0 4px 15px rgba(0, 123, 255, 0.1);
  border-color: #007bff;
}

/* Akordiyon Başlığı */
.accordion-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 20px;
  cursor: pointer;
  background-color: #fcfcfc;
  transition: background-color 0.2s ease;
}

.accordion-header:hover {
  background-color: #f0f8ff;
}

.header-info {
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}

.company-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: #2c3e50;
  margin: 0;
}

.role-text {
  font-size: 1rem;
  color: #007bff;
  font-weight: 500;
}

.header-meta {
  display: flex;
  align-items: center;
  gap: 15px;
}

.date-range {
  font-size: 0.9rem;
  color: #6c757d;
}

.toggle-icon {
  font-size: 0.8rem;
  color: #007bff;
  transition: transform 0.3s ease;
}

.accordion-item.is-active .toggle-icon {
  transform: rotate(180deg);
}

/* Akordiyon İçeriği */
.accordion-content {
  padding: 0 20px;
  max-height: 0; /* İçerik geçişi için başlangıçta sıfır olmalı */
  overflow: hidden;
  transition: max-height 0.4s ease-out, padding 0.4s ease-out; 
}

/* v-show kullanıldığı için height yerine padding/transition'ı deneyimlemesi zor olabilir. 
   Akordiyon animasyonunu CSS ile daha iyi kontrol etmek için `v-if` kullanıp, Vue'nun 
   transition bileşenini kullanmak daha profesyonel bir sonuç verir. 
   Ancak bu örnekte `v-show` ile basit bir görünüm sağlıyoruz. 
   
   Eğer gerçek bir animasyon istenirse, `max-height` yerine Vue Transition hook'ları kullanılmalıdır. */

.accordion-item.is-active .accordion-content {
  padding: 15px 20px 20px; /* İçerik açıldığında padding ekle */
  max-height: 500px; /* Yeterince büyük bir değer vererek içeriğin görünmesini sağla */
}

.description {
  margin-top: 0;
  color: #4a4a4a;
}

/* Küçük Ekranlar */
@media (max-width: 600px) {
  .accordion-header {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .header-meta {
    margin-top: 10px;
    align-self: flex-end;
  }
  
  .company-title {
    font-size: 1.2rem;
  }
  
  .date-range {
    order: 2; /* Tarihi ikonun sağından sola al */
  }

  .toggle-icon {
    margin-left: 10px;
    order: 1; /* İkonu sağa al */
  }
}
</style>