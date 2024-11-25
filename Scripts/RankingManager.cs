    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class RankingEntry
    {
        public string playerName;
        public int totalGoals;

        public RankingEntry(string playerName, int totalGoals)
        {
            this.playerName = playerName;
            this.totalGoals = totalGoals;
        }
    }

    [System.Serializable]
    public class RankingList
    {
        public RankingEntry[] entries;
    }

    public class RankingManager : MonoBehaviour
    {
        private const string RankingKey = "Ranking"; // Chave para armazenar o ranking
        public static RankingManager Instance { get; private set; }  // Instância estática

        public List<RankingEntry> ranking = new List<RankingEntry>();

        private void Awake()
        {
            // Garantir que só exista uma instância do RankingManager
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);  // Opcional: Mantém a instância entre cenas
            }
            else
            {
                Destroy(gameObject);
            }

            LoadRanking();
        }

        // Salva o ranking no PlayerPrefs
        public void SaveRanking()
        {
            string rankingJson = JsonUtility.ToJson(new RankingList { entries = ranking.ToArray() });
            PlayerPrefs.SetString(RankingKey, rankingJson);
            PlayerPrefs.Save();

            Debug.Log("JSON salvo: " + rankingJson); // Verifique o conteúdo do JSON salvo
        }

        // Carrega o ranking do PlayerPrefs
        public void LoadRanking()
        {
            if (PlayerPrefs.HasKey(RankingKey))
            {
                string rankingJson = PlayerPrefs.GetString(RankingKey);
                Debug.Log("JSON carregado: " + rankingJson); // Verifique o formato do JSON

                // Tente desserializar com JsonUtility
            
                    RankingList loadedRanking = JsonUtility.FromJson<RankingList>(rankingJson);
                    if (loadedRanking != null && loadedRanking.entries != null)
                    {
                        ranking = new List<RankingEntry>(loadedRanking.entries); // Converte o array para List
                    }
                    else
                    {
                        Debug.LogError("Falha ao desserializar o ranking. O formato do JSON pode estar errado.");
                    }
            
            }
            else
            {
                Debug.Log("Nenhum ranking encontrado.");
            }
        }

        // Adiciona uma nova entrada ao ranking
        public void AddRankingEntry(string playerName, int totalGoals)
        {
            RankingEntry newEntry = new RankingEntry(playerName, totalGoals);
            ranking.Add(newEntry);
            ranking.Sort((entry1, entry2) => entry2.totalGoals.CompareTo(entry1.totalGoals)); // Ordena em ordem decrescente
            if (ranking.Count > 10) // Limita a 10 jogadores
            {
                ranking.RemoveAt(ranking.Count - 1);
            }
            SaveRanking();
        }
    }
