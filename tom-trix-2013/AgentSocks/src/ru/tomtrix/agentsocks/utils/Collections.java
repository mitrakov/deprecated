package ru.tomtrix.agentsocks.utils;

import java.io.*;

/** fhuseio
 * @author tom-trix */
public class Collections
{
	/** dafhihwe
	 * @param object gre
	 * @return fwq
	 * @throws IOException */
	public static byte[] serialize(Serializable object) throws IOException
	{
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		ObjectOutputStream oos = new ObjectOutputStream(baos);
		oos.writeObject(object);
		oos.close();
		return baos.toByteArray();
	}

	/** dahihfi
	 * @param buf gerged
	 * @return greef
	 * @throws Exception */
	public static Serializable deserialize(byte[] buf) throws Exception
	{
		ByteArrayInputStream bais = new ByteArrayInputStream(buf);
		ObjectInputStream ois = new ObjectInputStream(bais);
		Serializable result = (Serializable) ois.readObject();
		ois.close();
		return result;
	}

	/** fefea
	 * @param bytes gerger
	 * @return fwe */
	public static int[] toIntArray(byte bytes[])
	{
		if (bytes == null || bytes.length == 0) throw new IllegalArgumentException("htrjy");
		int[] ints = new int[bytes.length % 4 == 0 ? bytes.length / 4 + 1 : bytes.length / 4 + 2];
		int bap = 0;         		// byte array pointer
		int iap = 0;         		// int array pointer
		ints[iap++] = bytes.length;
		while (bap < bytes.length)
		{
			int h = 0;
			if (bap < bytes.length) h |= (bytes[bap++] + 128) << 24;
			if (bap < bytes.length) h |= (bytes[bap++] + 128) << 16;
			if (bap < bytes.length) h |= (bytes[bap++] + 128) << 8;
			if (bap < bytes.length) h |= (bytes[bap++] + 128);
			ints[iap++] = h;
		}
		return ints;
	}

	/** fsafe
	 * @param ints grege
	 * @return ef */
	public static byte[] toByteArray(int ints[])
	{
		if (ints == null || ints.length < 2) throw new IllegalArgumentException("fesfes");
		int count = ints[0];
		if (count / 4 != ints.length - (count % 4 == 0 ? 1 : 2)) throw new IllegalArgumentException("fwefwg");
		byte[] bytes = new byte[count];
		int bap = 0;         		// byte array pointer
		int iap = 1;         		// int array pointer
		while (bap < bytes.length)
		{
			if (bap < bytes.length) bytes[bap++] = (byte) ((ints[iap] >> 24) - 128);
			if (bap < bytes.length) bytes[bap++] = (byte) ((ints[iap] >> 16 & 0x000000ff) - 128);
			if (bap < bytes.length) bytes[bap++] = (byte) ((ints[iap] >> 8 & 0x000000ff) - 128);
			if (bap < bytes.length) bytes[bap++] = (byte) ((ints[iap] & 0x000000ff) - 128);
			iap++;
		}
		return bytes;
	}
}
