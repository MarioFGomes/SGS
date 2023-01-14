using CodeReview.Business.Models.Produtos;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Infra.Data.Mappings
{
    internal class ProdutoConfig : EntityTypeConfiguration<Produto>
    {
        public ProdutoConfig()
        {
            HasKey(x => x.Id);

            Property(x=>x.Nome)
                .IsRequired()
                .HasMaxLength(100);

            Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            Property(x => x.Image)
                //.IsRequired()
                .HasMaxLength(1000);

            Property(x=>x.Valor)
                .IsRequired();

            HasRequired(X=>X.fornecedor)
                .WithMany(f=>f.produtos)
                .HasForeignKey(f=>f.FornecedorId);

        }
    }
}
