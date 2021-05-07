using Assets.Src.Evolution;

namespace Assets.Src.Interfaces
{
    public interface IGeneticConfigurable
    {
        
        GenomeWrapper Configure(GenomeWrapper genomeWrapper);
    }
}
