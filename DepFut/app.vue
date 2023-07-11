<template>
  <v-app>
    <v-toolbar width="100%" color="#121212" dark>
      <v-row align="center" justify="center">
        <v-img src="https://i.imgur.com/3FlLgMd.png" max-height="90"></v-img>
        <v-toolbar-title class="white--text">
          Bem-vindo ao software de gestão de jogadores do Vue Football Club
        </v-toolbar-title>
      </v-row>
    </v-toolbar>
    <v-data-table
      :headers="headers"
      :items="players"
      :sort-by="[{ key: 'nome', order: 'asc' }]"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Elenco</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ props }">
              <v-btn color="primary" dark class="mb-2" v-bind="props">
                Novo jogador
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span>{{ formTitle }}</span>
              </v-card-title>

              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.nome"
                        label="Nome do jogador"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.idade"
                        label="Idade"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.posicao"
                        label="Posição"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.nacionalidade"
                        label="Nacionalidade"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.gols"
                        label="Gols"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.assistencias"
                        label="Assistências"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" variant="text" @click="close">
                  Cancelar
                </v-btn>
                <v-btn color="blue-darken-1" variant="text" @click="save">
                  Salvar
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="text-h5"
                >Você tem certeza que você quer deletar esse item?</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" variant="text" @click="closeDelete"
                  >Cancelar</v-btn
                >
                <v-btn
                  color="blue-darken-1"
                  variant="text"
                  @click="deleteItemConfirm"
                  >OK</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon size="small" class="me-2" @click="editItem(item.raw)">
          mdi-pencil
        </v-icon>
        <v-icon size="small" @click="deleteItem(item.raw)"> mdi-delete </v-icon>
      </template>
      <template v-slot:no-data>
        <v-chip>Nenhum jogador disponível</v-chip>
      </template>
    </v-data-table>
    <v-footer color="#121212" padless>
      <v-spacer></v-spacer>
      <v-card flat>
        <v-card-text class="white--text text-center">
          <div>
            <span
              >Desenvolvido por Nathan S. Sodré - © {{ getCurrentYear() }}</span
            >
          </div>
          <div>
            <v-icon size="24">mdi-email</v-icon>
            <span>nathanssodre@gmail.com</span>
          </div>
          <div>
            <v-btn icon color="white">
              <v-icon size="24">mdi-facebook</v-icon>
            </v-btn>
            <v-btn icon color="white">
              <v-icon size="24">mdi-twitter</v-icon>
            </v-btn>
            <v-btn icon color="white">
              <v-icon size="24">mdi-instagram</v-icon>
            </v-btn>
          </div>
        </v-card-text>
      </v-card>
      <v-spacer></v-spacer>
    </v-footer>
  </v-app>
</template>
<script>
import axios from "axios";

export default {
  data: () => ({
    dialog: false,
    dialogDelete: false,
    headers: [
      {
        title: "Nome do jogador",
        align: "start",
        sortable: true,
        key: "nome",
      },
      { title: "Idade", key: "idade" },
      { title: "Posição", key: "posicao" },
      { title: "Nacionalidade", key: "nacionalidade" },
      { title: "Gols", key: "gols" },
      { title: "Assistências", key: "assistencias" },
      { title: "Actions", key: "actions", sortable: false },
    ],
    players: [],
    editedIndex: -1,
    editedItem: {
      nome: "",
      idade: 0,
      posicao: "",
      nacionalidade: "",
      gols: 0,
      assistencias: 0,
    },
    defaultItem: {
      nome: "",
      idade: 0,
      posicao: "",
      nacionalidade: "",
      gols: 0,
      assistencias: 0,
    },
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "Novo jogador" : "Editar jogador";
    },
  },

  watch: {
    dialog(val) {
      val || this.close();
    },
    dialogDelete(val) {
      val || this.closeDelete();
    },
  },

  created() {
    this.getPlayers();
  },

  methods: {
    getPlayers() {
      axios
        .get("https://localhost:7091/api/Players")
        .then((response) => {
          this.players = response.data;
        })
        .catch((error) => {
          console.error(error);
        });
    },

    editItem(item) {
      this.editedIndex = this.players.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      this.editedIndex = this.players.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },

    deleteItemConfirm() {
      axios
        .delete(`https://localhost:7091/api/Players/${this.editedItem.id}`)
        .then((response) => {
          this.players.splice(this.editedIndex, 1);
          this.closeDelete();
        })
        .catch((error) => {
          console.error(error);
        });
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },

    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },

    save() {
      if (this.editedIndex > -1) {
        axios
          .put(
            `https://localhost:7091/api/Players/${this.editedItem.id}`,
            this.editedItem
          )
          .then((response) => {
            Object.assign(this.players[this.editedIndex], this.editedItem);
            this.close();
          })
          .catch((error) => {
            console.error(error);
          });
      } else {
        axios
          .post("https://localhost:7091/api/Players", this.editedItem)
          .then((response) => {
            this.players.push(response.data);
            this.close();
          })
          .catch((error) => {
            console.error(error);
          });
      }
    },
    getCurrentYear() {
      const date = new Date();
      return date.getFullYear();
    },
  },
};
</script>
